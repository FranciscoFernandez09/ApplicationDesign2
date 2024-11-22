using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Requests.Filters;

namespace SmartHome.WebApi.Controllers;

[ApiController]
[AuthenticationFilter]
public sealed class UserController(IUserService service) : SmartHomeControllerBase
{
    [HttpGet]
    [Route("devices")]
    public IActionResult GetDevices([FromQuery] FilterDeviceRequest request)
    {
        var dto = new FilterDeviceArgs(request.Name, request.Model, request.CompanyName, request.Type,
            request.Offset, request.Limit);
        List<ShowDeviceDto> devices = service.GetDevices(dto);
        return Ok(devices);
    }

    [HttpGet]
    [Route("deviceTypes")]
    public IActionResult GetDevicesTypes()
    {
        List<string> devicesTypes = service.GetDevicesTypes();
        return Ok(devicesTypes);
    }

    [HttpPatch]
    [Route("users/profileImage")]
    public ActionResult ModifyProfileImage([FromBody] UpdateProfileImageRequest request)
    {
        User user = GetLoggedUser();
        service.ModifyProfileImage(user, request.ProfileImage);

        return Ok("Profile image modified successfully.");
    }
}
