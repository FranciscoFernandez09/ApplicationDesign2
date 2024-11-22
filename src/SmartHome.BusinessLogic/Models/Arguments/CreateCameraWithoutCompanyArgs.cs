using SmartHome.BusinessLogic.Domain.SmartDevices;

namespace SmartHome.BusinessLogic.Models.Arguments;

public sealed class CreateCameraWithoutCompanyArgs(
    string? name,
    string? model,
    string? description,
    List<DeviceImage> images,
    bool? hasExternalUse,
    bool? hasInternalUse,
    bool? motionDetection,
    bool? personDetection)
    : CreateSmartDeviceWithoutCompanyArgs(name, model, description, "Camera", images)
{
    public readonly bool HasExternalUse = hasExternalUse ?? throw new ArgumentNullException(nameof(hasExternalUse));
    public readonly bool HasInternalUse = hasInternalUse ?? throw new ArgumentNullException(nameof(hasInternalUse));
    public readonly bool MotionDetection = motionDetection ?? throw new ArgumentNullException(nameof(motionDetection));
    public readonly bool PersonDetection = personDetection ?? throw new ArgumentNullException(nameof(personDetection));
}
