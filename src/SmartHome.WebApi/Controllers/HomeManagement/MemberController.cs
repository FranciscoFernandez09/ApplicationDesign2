using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces.HomeManagement;
using SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests.Filters;

namespace SmartHome.WebApi.Controllers.HomeManagement;

[ApiController]
[AuthenticationFilter]
public sealed class MemberController(IMemberService service) : SmartHomeControllerBase
{
    [HttpPatch]
    [AuthorizationFilter(Constant.ActivateNotification)]
    [Route("members/{memberId}/activateNotification")]
    public ActionResult ActivateMemberNotification([FromRoute] Guid? memberId)
    {
        User user = GetLoggedUser();
        const bool state = true;
        service.ChangeMemberNotificationStateTo(user, memberId, state);

        return Ok("Member notification activated successfully.");
    }

    [HttpPatch]
    [AuthorizationFilter(Constant.DeactivateNotification)]
    [Route("members/{memberId}/deactivateNotification")]
    public ActionResult DeactivateMemberNotification([FromRoute] Guid? memberId)
    {
        User user = GetLoggedUser();
        const bool state = false;
        service.ChangeMemberNotificationStateTo(user, memberId, state);

        return Ok("Member notification deactivated successfully.");
    }

    [HttpGet]
    [AuthorizationFilter(Constant.GetNotifications)]
    [Route("notifications")]
    public ActionResult GetNotifications([FromQuery] FilterNotificationRequest request)
    {
        User user = GetLoggedUser();
        var dto = new FilterNotificationsArgs(user, request.DeviceType, request.Date, request.IsRead);
        List<ShowNotificationDto> notifications = service.GetNotifications(dto);

        return Ok(notifications);
    }
}
