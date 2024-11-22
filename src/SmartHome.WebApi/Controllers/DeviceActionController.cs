using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Controllers;

[ApiController]
public sealed class DeviceActionController(IDeviceActionService deviceActionService) : SmartHomeControllerBase
{
    [HttpPost]
    [Route("cameras/{hardwareId}/personDetection")]
    public IActionResult PersonDetectionAction([FromRoute] Guid? hardwareId)
    {
        User user = GetLoggedUser();
        deviceActionService.PersonDetectionAction(hardwareId, user);
        return Ok("Notification of person detection sent.");
    }

    [HttpPost]
    [Route("cameras/{hardwareId}/movementDetection")]
    public IActionResult CameraMovementDetectionAction([FromRoute] Guid? hardwareId)
    {
        deviceActionService.CameraMovementDetectionAction(hardwareId);
        return Ok("Notification of movement detection sent.");
    }

    [HttpPatch]
    [Route("windowSensors/{hardwareId}/open")]
    public IActionResult OpenWindowSensor([FromRoute] Guid? hardwareId)
    {
        const bool isActive = true;
        deviceActionService.ChangeWindowSensorStateTo(hardwareId, isActive);
        return Ok("Notification of sensor state open sent.");
    }

    [HttpPatch]
    [Route("windowSensors/{hardwareId}/close")]
    public IActionResult CloseWindowSensor([FromRoute] Guid? hardwareId)
    {
        const bool isActive = false;
        deviceActionService.ChangeWindowSensorStateTo(hardwareId, isActive);
        return Ok("Notification of sensor state close sent.");
    }

    [HttpPatch]
    [AuthenticationFilter]
    [Route("smartLamps/{hardwareId}/turnOn")]
    public IActionResult TurnOnSmartLamp([FromRoute] Guid hardwareId)
    {
        const bool isActive = true;
        User user = GetLoggedUser();
        deviceActionService.ChangeSmartLampStateTo(user, hardwareId, isActive);

        return Ok("Smart lamp turned on successfully.");
    }

    [HttpPatch]
    [AuthenticationFilter]
    [Route("smartLamps/{hardwareId}/turnOff")]
    public IActionResult TurnOffSmartLamp([FromRoute] Guid hardwareId)
    {
        const bool isActive = false;
        User user = GetLoggedUser();
        deviceActionService.ChangeSmartLampStateTo(user, hardwareId, isActive);

        return Ok("Smart lamp turned off successfully.");
    }

    [HttpPost]
    [Route("motionSensors/{hardwareId}/movementDetection")]
    public IActionResult MotionSensorMovementDetection([FromRoute] Guid hardwareId)
    {
        deviceActionService.MotionSensorMovementDetection(hardwareId);

        return Ok("Notification of movement detection sent.");
    }
}
