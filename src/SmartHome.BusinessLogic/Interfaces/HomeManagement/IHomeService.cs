using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Interfaces.HomeManagement;

public interface IHomeService
{
    public void CreateHome(CreateHomeArgs args);
    public void AddMember(User currentUser, string? memberEmail, Guid? homeId);
    public void AddHomePermission(AddHomePermissionArgs args);
    public void AddSmartDevice(User currentUser, Guid? homeId, Guid? deviceId);
    public List<ShowHomeMemberDto> GetHomeMembers(User currentUser, Guid? homeId);
    public List<ShowHomeDeviceDto> GetHomeDevices(User currentUser, Guid? homeId, Guid? room);
    public void ModifyHomeName(User currentUser, Guid? homeId, string? name);
    public List<ShowHomeDto> GetMineHomes(User currentUser);
    public List<ShowHomeDto> GetHomesWhereIMember(User currentUser);
    public List<ShowHomePermissionDto> GetHomePermissions();
}
