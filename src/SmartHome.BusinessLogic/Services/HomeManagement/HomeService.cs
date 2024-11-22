using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.EFCoreClasses;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Interfaces.HomeManagement;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Services.HomeManagement;

public sealed class HomeService(
    IRepository<User> userRepository,
    IRepository<Home> homeRepository,
    IRepository<SmartDevice> smartDeviceRepository,
    IRepository<HomeDevice> homeDeviceRepository,
    IRepository<HomeMember> homeMemberRepository,
    IRepository<HomePermission> homePermissionRepository,
    IRepository<MemberHomePermission> memberHomePermissionRepository)
    : IHomeService
{
    public void CreateHome(CreateHomeArgs args)
    {
        var home = new Home(args);

        var member = new HomeMember(home, args.Owner);
        member.OwnerMemberSettings();
        homeRepository.Add(home);
        homeMemberRepository.Add(member);
    }

    public void AddMember(User currentUser, string? memberEmail, Guid? homeId)
    {
        ThrowExceptionWhenParameterIsNullOrEmpty(homeId, nameof(homeId));

        if (string.IsNullOrEmpty(memberEmail))
        {
            throw new ArgumentNullException(nameof(memberEmail));
        }

        Home home = ValidatedAndGetHomeToAddMember(homeId);

        if (!home.IsOwner(currentUser))
        {
            throw new UnauthorizedAccessException("User does not have permission to add member.");
        }

        User? member = userRepository.Get(u => u.Email == memberEmail)!;
        if (member == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        if (home.IsMember(member))
        {
            throw new InvalidOperationException("User is already a member of the home.");
        }

        if (!member.IsHomeOwnerRole())
        {
            throw new InvalidOperationException("User does not have owner home role.");
        }

        var homeMember = new HomeMember(home, member);
        homeMemberRepository.Add(homeMember);
    }

    public void AddHomePermission(AddHomePermissionArgs args)
    {
        HomeMember? homeMember = homeMemberRepository.Get(hm => hm.Id == args.MemberId);
        if (homeMember == null)
        {
            throw new InvalidOperationException("Member not found.");
        }

        Home home = homeMember.Home;
        if (!home.IsOwner(args.User))
        {
            throw new UnauthorizedAccessException("User does not have permission to add member permission.");
        }

        User user = homeMember.User;
        var alreadyHasPermission = memberHomePermissionRepository.Exists(mhp =>
            mhp.MemberId == args.MemberId && mhp.PermissionId == args.PermissionId);
        if (alreadyHasPermission || home.IsOwner(user))
        {
            throw new InvalidOperationException("User already has this permission.");
        }

        HomePermission? homePermission = homePermissionRepository.Get(hp => hp.Id == args.PermissionId);
        if (homePermission == null)
        {
            throw new InvalidOperationException("Permission not found.");
        }

        memberHomePermissionRepository.Add(new MemberHomePermission(homeMember, homePermission));
    }

    public void AddSmartDevice(User currentUser, Guid? homeId, Guid? deviceId)
    {
        ThrowExceptionWhenParameterIsNullOrEmpty(homeId, nameof(homeId));
        ThrowExceptionWhenParameterIsNullOrEmpty(deviceId, nameof(deviceId));

        HomeMember? homeMember = homeMemberRepository.Get(hm => hm.HomeId == homeId && hm.UserId == currentUser.Id);
        if (homeMember == null)
        {
            throw new InvalidOperationException("User is not home member.");
        }

        Home home = ValidateAndGetHome(homeId);

        var hasHomePermission = homeMember.HasHomePermission(Constant.AddSmartDeviceId);

        if (!home.IsOwner(currentUser) && !hasHomePermission)
        {
            throw new UnauthorizedAccessException("User does not have permission to add smart device.");
        }

        SmartDevice device = ValidateAndGetDevice(deviceId);
        ValidateDeviceNotAddedInHome(deviceId, home);
        var homeDevice = new HomeDevice(home, device);

        homeDeviceRepository.Add(homeDevice);
    }

    public List<ShowHomeMemberDto> GetHomeMembers(User currentUser, Guid? homeId)
    {
        ThrowExceptionWhenParameterIsNullOrEmpty(homeId, nameof(homeId));

        Home home = ValidateAndGetHome(homeId);
        if (!home.IsOwner(currentUser))
        {
            throw new UnauthorizedAccessException("User does not have permission to get home members.");
        }

        List<HomeMember> members = homeMemberRepository.GetAll(hm => hm.HomeId == homeId);

        return (from m in members
                let userData = m.GetUserData()
                select new ShowHomeMemberDto(m.Id, userData[0], userData[1], userData[2], m.ShouldNotify, userData[3]))
            .ToList();
    }

    public List<ShowHomeDeviceDto> GetHomeDevices(User currentUser, Guid? homeId, Guid? roomId)
    {
        ThrowExceptionWhenParameterIsNullOrEmpty(homeId, nameof(homeId));

        Home home = ValidateAndGetHome(homeId);

        ValidateUserToGetHomeDevices(currentUser, Constant.GetHomeDevicesId, home);

        var roomIdIsNullOrEmpty = !roomId.HasValue || roomId == Guid.Empty;
        List<HomeDevice> devices = homeDeviceRepository.GetAll(hd =>
            hd.HomeId == homeId && (roomIdIsNullOrEmpty || (hd.Room != null && hd.Room.Id == roomId)));

        List<ShowHomeDeviceDto> result = [];
        result.AddRange(from device in devices
                        let userData = device.GetNameAndMainImage()
                        let deviceName = device.Name
                        let mainImage = userData[1]
                        select new ShowHomeDeviceDto(device.Id, deviceName, device.GetDeviceType(), device.IsActive,
                            device.GetDeviceModel(), mainImage, device.IsConnected));

        return result;
    }

    public void ModifyHomeName(User currentUser, Guid? homeId, string? name)
    {
        ThrowExceptionWhenParameterIsNullOrEmpty(homeId, nameof(homeId));
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        Home? home = homeRepository.Get(h => h.Id == homeId);
        if (home == null)
        {
            throw new InvalidOperationException("Home not found.");
        }

        if (!home.IsMember(currentUser))
        {
            throw new UnauthorizedAccessException("User does not is member of the home.");
        }

        home.Name = name;
        homeRepository.Update(home);
    }

    public List<ShowHomeDto> GetMineHomes(User currentUser)
    {
        List<Home> homes = homeRepository.GetAll(h => h.Owner.Id == currentUser.Id);

        return homes.Select(h => new ShowHomeDto(h.Id, h.Name)).ToList();
    }

    public List<ShowHomeDto> GetHomesWhereIMember(User currentUser)
    {
        List<HomeMember> homeMembers = homeMemberRepository.GetAll(h => h.UserId == currentUser.Id);
        var homes = homeMembers.Select(hm => hm.Home).ToList();

        return homes.Select(h => new ShowHomeDto(h.Id, h.Name)).ToList();
    }

    public List<ShowHomePermissionDto> GetHomePermissions()
    {
        List<HomePermission> permissions = homePermissionRepository.GetAll();
        var permissionsDto = permissions.Select(p => new ShowHomePermissionDto(p.Id, p.Name)).ToList();
        return permissionsDto;
    }

    private void ValidateDeviceNotAddedInHome(Guid? deviceId, Home home)
    {
        var existsDevice = homeDeviceRepository.Exists(hd => hd.DeviceId == deviceId && hd.HomeId == home.Id);
        if (existsDevice)
        {
            throw new InvalidOperationException("Device is already added in the home.");
        }
    }

    private void ValidateUserToGetHomeDevices(User currentUser, Guid permission, Home home)
    {
        if (IsValidOwner(currentUser, home))
        {
            return;
        }

        HomeMember? member = homeMemberRepository.Get(hm => hm.HomeId == home.Id && hm.UserId == currentUser.Id);
        if (member == null || !member.HasHomePermission(permission))
        {
            throw new UnauthorizedAccessException("User does not have permission to get home devices.");
        }
    }

    private static bool IsValidOwner(User currentUser, Home home)
    {
        return home.IsOwner(currentUser);
    }

    private Home ValidatedAndGetHomeToAddMember(Guid? homeId)
    {
        Home home = ValidateAndGetHome(homeId);

        if (home.IsFull())
        {
            throw new InvalidOperationException("Home member capacity is full.");
        }

        return home;
    }

    private Home ValidateAndGetHome(Guid? homeId)
    {
        Home? home = homeRepository.Get(h => h.Id == homeId);
        if (home == null)
        {
            throw new InvalidOperationException("Home not found.");
        }

        return home;
    }

    private SmartDevice ValidateAndGetDevice(Guid? deviceId)
    {
        SmartDevice? device = smartDeviceRepository.Get(d => d.Id == deviceId);
        if (device == null)
        {
            throw new InvalidOperationException("Device not found.");
        }

        return smartDeviceRepository.Get(d => d.Id == deviceId)!;
    }

    private static void ThrowExceptionWhenParameterIsNullOrEmpty(Guid? parameter, string parameterName)
    {
        if (parameter == null || parameter == Guid.Empty)
        {
            throw new ArgumentNullException(parameterName);
        }
    }
}
