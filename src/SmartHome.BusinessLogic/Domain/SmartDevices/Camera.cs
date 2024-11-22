using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

namespace SmartHome.BusinessLogic.Domain.SmartDevices;

public sealed class Camera : SmartDevice
{
    public Camera()
    {
        DeviceType = DeviceTypeEnum.Camera;
    }

    public Camera(CreateCameraArgs args)
        : base(args)
    {
        HasExternalUse = args.HasExternalUse;
        HasInternalUse = args.HasInternalUse;
        MotionDetection = args.MotionDetection;
        PersonDetection = args.PersonDetection;
        DeviceType = DeviceTypeEnum.Camera;
    }

    public bool HasExternalUse { get; init; }
    public bool HasInternalUse { get; init; }
    public bool MotionDetection { get; init; }
    public bool PersonDetection { get; init; }
}
