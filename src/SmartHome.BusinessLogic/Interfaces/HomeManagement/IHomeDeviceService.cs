using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces.HomeManagement;

public interface IHomeDeviceService
{
    public void ConnectDevice(User currentUser, Guid? hardwareId);
    public void DisconnectDevice(User currentUser, Guid? hardwareId);
    public void ModifyHomeDeviceName(User currentUser, Guid? hardwareId, string? name);
}
