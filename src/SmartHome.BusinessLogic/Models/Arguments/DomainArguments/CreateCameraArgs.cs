using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.SmartDevices;

namespace SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

public sealed class CreateCameraArgs(
    string? name,
    string? model,
    string? description,
    Company? companyOwner,
    List<DeviceImage> images,
    bool? hasExternalUse,
    bool? hasInternalUse,
    bool? motionDetection,
    bool? personDetection)
    : CreateSmartDeviceArgs(name, model, description, companyOwner, "Camera", images)
{
    public readonly bool HasExternalUse = hasExternalUse ?? throw new ArgumentNullException(nameof(hasExternalUse));
    public readonly bool HasInternalUse = hasInternalUse ?? throw new ArgumentNullException(nameof(hasInternalUse));
    public readonly bool MotionDetection = motionDetection ?? throw new ArgumentNullException(nameof(motionDetection));
    public readonly bool PersonDetection = personDetection ?? throw new ArgumentNullException(nameof(personDetection));
}
