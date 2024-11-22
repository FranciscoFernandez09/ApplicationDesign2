namespace SmartHome.WebApi.Requests;

public sealed class HardwareIdRequest(Guid? hardwareId)
{
    public Guid? HardwareId { get; set; } = hardwareId;
}
