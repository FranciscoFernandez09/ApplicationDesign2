namespace SmartHome.WebApi.Requests;

public sealed class AddDeviceRequest(Guid? deviceId)
{
    public Guid? DeviceId { get; set; } = deviceId;
}
