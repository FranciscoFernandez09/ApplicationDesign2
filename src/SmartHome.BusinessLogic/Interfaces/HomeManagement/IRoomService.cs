using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Interfaces.HomeManagement;

public interface IRoomService
{
    public void AddAndSave(User currentUser, Guid? homeId, string? name);
    public void AddDeviceAndSave(User currentUser, Guid? roomId, Guid? hardwareId);
    public List<ShowRoomDto> GetRooms(Guid? homeId, User currentUser);
}
