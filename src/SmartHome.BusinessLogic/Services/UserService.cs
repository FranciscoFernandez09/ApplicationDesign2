using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Services;

public sealed class UserService(
    IRepository<User> userRepository,
    IRepository<Role> roleRepository,
    IRepository<SmartDevice> deviceRepository)
    : IUserService, IUnknownUserService
{
    public void CreateHomeOwner(CreateUserArgs args)
    {
        Role homeOwner = roleRepository.Get(r => r.Id == Constant.HomeOwnerRoleId)!;
        args.Role = homeOwner;

        ThrowExceptionWhenParameterIsNullOrEmpty(args.ProfileImage, nameof(args.ProfileImage));

        if (ExistsEmail(args.Email))
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        var user = new User(args);
        userRepository.Add(user);
    }

    public List<ShowDeviceDto> GetDevices(FilterDeviceArgs args)
    {
        List<SmartDevice> devices = deviceRepository.GetAll(d =>
                (args.DeviceType == null || d.DeviceType == args.DeviceType) &&
                (string.IsNullOrEmpty(args.Name) || d.Name == args.Name) &&
                (args.Model == null || d.Model == args.Model) &&
                (string.IsNullOrEmpty(args.CompanyName) || d.CompanyOwner.Name == args.CompanyName), args.Offset,
            args.Limit);

        var devicesDto = devices.Select(MapDeviceToDto).ToList();

        return devicesDto;
    }

    public List<string> GetDevicesTypes()
    {
        return Enum.GetNames(typeof(DeviceTypeEnum)).ToList();
    }

    public void ModifyProfileImage(User user, string? profileImage)
    {
        ThrowExceptionWhenParameterIsNullOrEmpty(profileImage, nameof(profileImage));

        user.ModifyProfileImage(profileImage);
        userRepository.Update(user);
    }

    private static ShowDeviceDto MapDeviceToDto(SmartDevice device)
    {
        return new ShowDeviceDto(device.Id, device.Name, device.Model, device.GetMainImage().ImageUrl,
            device.CompanyOwner.Name);
    }

    private static void ThrowExceptionWhenParameterIsNullOrEmpty(string? parameter, string parameterName)
    {
        if (string.IsNullOrEmpty(parameter))
        {
            throw new ArgumentNullException(parameterName);
        }
    }

    private bool ExistsEmail(string email)
    {
        return userRepository.Exists(u => u.Email == email);
    }
}
