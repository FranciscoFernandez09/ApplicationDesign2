using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Interfaces.HomeManagement;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Services.HomeManagement;

public sealed class RoomService(
    IRepository<Home> homeRepository,
    IRepository<HomeDevice> homeDeviceRepository,
    IRepository<HomeMember> homeMemberRepository,
    IRepository<Room> roomRepository)
    : IRoomService
{
    public void AddAndSave(User currentUser, Guid? homeId, string? name)
    {
        ThrowExceptionWhenParameterIsNullOrEmpty(homeId, nameof(homeId));
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        Home home = ValidateAndGetHome(homeId);
        if (!IsValidOwner(currentUser, home))
        {
            throw new UnauthorizedAccessException("User does not is owner of the home.");
        }

        var room = new Room(name, home);
        roomRepository.Add(room);
    }

    public void AddDeviceAndSave(User currentUser, Guid? roomId, Guid? hardwareId)
    {
        ThrowExceptionWhenParameterIsNullOrEmpty(roomId, nameof(roomId));
        ThrowExceptionWhenParameterIsNullOrEmpty(hardwareId, nameof(hardwareId));

        Room? room = roomRepository.Get(r => r.Id == roomId);
        if (room == null)
        {
            throw new InvalidOperationException("Room not found.");
        }

        HomeDevice? homeDevice = homeDeviceRepository.Get(hd => hd.Id == hardwareId);
        if (homeDevice == null)
        {
            throw new InvalidOperationException("Device is not added to home.");
        }

        ValidateMemberAndHomeDeviceToAddedInRoom(currentUser, room, homeDevice);

        homeDevice.Room = room;
        homeDeviceRepository.Update(homeDevice);
    }

    public List<ShowRoomDto> GetRooms(Guid? homeId, User currentUser)
    {
        ThrowExceptionWhenParameterIsNullOrEmpty(homeId, nameof(homeId));

        HomeMember? member = homeMemberRepository.Get(hm => hm.HomeId == homeId && hm.UserId == currentUser.Id);
        if (member == null)
        {
            throw new UnauthorizedAccessException("User does not is member of the home.");
        }

        List<Room> rooms = roomRepository.GetAll(r => r.Home.Id == homeId);
        return rooms.Select(r => new ShowRoomDto(r.Id, r.Name)).ToList();
    }

    private void ValidateMemberAndHomeDeviceToAddedInRoom(User user, Room room, HomeDevice homeDevice)
    {
        if (homeDevice.GetHomeId() != room.GetHomeId())
        {
            throw new InvalidOperationException("Device is not in the same home as the room.");
        }

        if (homeDevice.Room != null && homeDevice.GetRoomId() == room.Id)
        {
            throw new InvalidOperationException("Device is already added to the room.");
        }

        HomeMember? homeMember = homeMemberRepository.Get(hm => hm.HomeId == room.Home.Id && hm.UserId == user.Id);
        if (homeMember == null)
        {
            throw new UnauthorizedAccessException("User does not is member of the home.");
        }

        Home home = homeDevice.Home;
        if (!home.IsOwner(user) && !homeMember.HasHomePermission(Constant.AddDeviceToRoomId))
        {
            throw new UnauthorizedAccessException("User does not have permission to add device to room.");
        }
    }

    private static void ThrowExceptionWhenParameterIsNullOrEmpty(Guid? parameter, string parameterName)
    {
        if (parameter == null || parameter == Guid.Empty)
        {
            throw new ArgumentNullException(parameterName);
        }
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

    private static bool IsValidOwner(User currentUser, Home home)
    {
        return home.IsOwner(currentUser);
    }
}
