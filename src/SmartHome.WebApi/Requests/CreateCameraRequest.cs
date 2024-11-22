namespace SmartHome.WebApi.Requests;

public sealed class CreateCameraRequest(
    string? name,
    string? model,
    string? description,
    bool? hasExternalUse,
    bool? hasInternalUse,
    bool? motionDetection,
    bool? personDetection,
    List<DeviceImageRequest> images)
    : CreateDeviceRequest(name, model, description, images)
{
    public bool? HasExternalUse { get; set; } = hasExternalUse;
    public bool? HasInternalUse { get; set; } = hasInternalUse;
    public bool? MotionDetection { get; set; } = motionDetection;
    public bool? PersonDetection { get; set; } = personDetection;
}
