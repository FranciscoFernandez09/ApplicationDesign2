using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces.HomeManagement;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;

namespace SmartHome.WebApi.Controllers.HomeManagement;

[ApiController]
[AuthenticationFilter]
[Route("homes")]
public sealed class HomeController(IHomeService service) : SmartHomeControllerBase
{
    [HttpPost]
    [AuthorizationFilter(Constant.CreateHome)]
    public ActionResult CreateHome([FromBody] CreateHomeRequest request)
    {
        User user = GetLoggedUser();
        var args = new CreateHomeArgs(request.AddressStreet, request.AddressNumber, request.Latitude, request.Longitude,
            request.MaxMembers, request.Name, user);
        service.CreateHome(args);

        return Ok("Home created successfully.");
    }

    [HttpPost]
    [AuthorizationFilter(Constant.AddMember)]
    [Route("{homeId}/members")]
    public ActionResult AddMember([FromBody] AddMemberRequest request, [FromRoute] Guid homeId)
    {
        User user = GetLoggedUser();
        service.AddMember(user, request.MemberEmail, homeId);

        return Ok("Home member added successfully.");
    }

    [HttpPost]
    [AuthorizationFilter(Constant.AddHomePermission)]
    [Route("{memberId}/permissions")]
    public ActionResult AddHomePermission([FromBody] AddHomePermissionRequest request, [FromRoute] Guid memberId)
    {
        User user = GetLoggedUser();
        var args = new AddHomePermissionArgs(user, memberId, request.PermissionId);
        service.AddHomePermission(args);

        return Ok("Home permission added successfully.");
    }

    [HttpPost]
    [AuthorizationFilter(Constant.AddSmartDevice)]
    [Route("{homeId}/devices")]
    public ActionResult AddSmartDevice([FromBody] AddDeviceRequest request, [FromRoute] Guid? homeId)
    {
        User user = GetLoggedUser();
        service.AddSmartDevice(user, homeId, request.DeviceId);

        return Ok("Smart device added successfully.");
    }

    [HttpGet]
    [AuthorizationFilter(Constant.GetHomeMembers)]
    [Route("{homeId}/members")]
    public ActionResult GetHomeMembers(Guid? homeId)
    {
        User user = GetLoggedUser();
        List<ShowHomeMemberDto> members = service.GetHomeMembers(user, homeId);

        return Ok(members);
    }

    [HttpGet]
    [AuthorizationFilter(Constant.GetHomeDevices)]
    [Route("{homeId}/devices")]
    public ActionResult GetHomeDevices(Guid? homeId, [FromQuery] Guid? room)
    {
        User user = GetLoggedUser();
        List<ShowHomeDeviceDto> devices = service.GetHomeDevices(user, homeId, room);

        return Ok(devices);
    }

    [HttpPatch]
    [AuthorizationFilter(Constant.ModifyHomeName)]
    [Route("{homeId}/name")]
    public ActionResult ModifyHomeName([FromBody] UpdateNameRequest request, [FromRoute] Guid homeId)
    {
        User user = GetLoggedUser();
        service.ModifyHomeName(user, homeId, request.Name);

        return Ok("Home name modified successfully.");
    }

    [HttpGet]
    [AuthorizationFilter(Constant.GetHomes)]
    [Route("mine")]
    public ActionResult GetMineHomes()
    {
        User user = GetLoggedUser();
        List<ShowHomeDto> homes = service.GetMineHomes(user);

        return Ok(homes);
    }

    [HttpGet]
    [AuthorizationFilter(Constant.GetHomes)]
    [Route("member")]
    public ActionResult GetHomesWhereIMember()
    {
        User user = GetLoggedUser();
        List<ShowHomeDto> homes = service.GetHomesWhereIMember(user);

        return Ok(homes);
    }

    [HttpGet]
    [AuthorizationFilter(Constant.AddHomePermission)]
    [Route("permissions")]
    public ActionResult GetPermissions()
    {
        List<ShowHomePermissionDto> permissions = service.GetHomePermissions();

        return Ok(permissions);
    }
}
