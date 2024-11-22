using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces.HomeManagement;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;

namespace SmartHome.WebApi.Controllers.HomeManagement;

[ApiController]
[AuthenticationFilter]
[Route("hardwares/{hardwareId}")]
public sealed class HomeDeviceController(IHomeDeviceService service) : SmartHomeControllerBase
{
    [HttpPatch]
    [AuthorizationFilter(Constant.AddSmartDevice)]
    [Route("connection")]
    public ActionResult ConnectDevice([FromRoute] Guid hardwareId)
    {
        User user = GetLoggedUser();
        service.ConnectDevice(user, hardwareId);

        return Ok("Device connected successfully.");
    }

    [HttpPatch]
    [AuthorizationFilter(Constant.AddSmartDevice)]
    [Route("disconnection")]
    public ActionResult DisconnectDevice([FromRoute] Guid hardwareId)
    {
        User user = GetLoggedUser();
        service.DisconnectDevice(user, hardwareId);

        return Ok("Device disconnected successfully.");
    }

    [HttpPatch]
    [AuthorizationFilter(Constant.ModifyHomeDeviceName)]
    [Route("name")]
    public ActionResult ModifyHomeDeviceName([FromBody] UpdateNameRequest request,
        [FromRoute] Guid hardwareId)
    {
        User user = GetLoggedUser();
        service.ModifyHomeDeviceName(user, hardwareId, request.Name);

        return Ok("Home device name modified successfully.");
    }
}
