using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;

public interface IDeviceActionService
{
    void PersonDetectionAction(Guid? hardwareId, User? user);
    void CameraMovementDetectionAction(Guid? hardwareId);
    void ChangeWindowSensorStateTo(Guid? hardwareId, bool isActive);
    void ChangeSmartLampStateTo(User user, Guid? hardwareId, bool isActive);
    void MotionSensorMovementDetection(Guid? hardwareId);
}
