using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces.HomeManagement;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;

namespace SmartHome.WebApi.Controllers.HomeManagement;

[ApiController]
[AuthenticationFilter]
public sealed class RoomController(IRoomService service) : SmartHomeControllerBase
{
    [HttpPost]
    [AuthorizationFilter(Constant.AddRoom)]
    [Route("homes/{homeId}/rooms")]
    public ActionResult AddRoom([FromBody] AddRoomRequest request, [FromRoute] Guid homeId)
    {
        User user = GetLoggedUser();
        service.AddAndSave(user, homeId, request.Name);

        return Ok("Room added successfully.");
    }

    [HttpPost]
    [AuthorizationFilter(Constant.AddDeviceToRoom)]
    [Route("rooms/{roomId}/devices")]
    public ActionResult AddDeviceToRoom([FromBody] HardwareIdRequest request, [FromRoute] Guid roomId)
    {
        User user = GetLoggedUser();
        service.AddDeviceAndSave(user, roomId, request.HardwareId);

        return Ok("Device added to room successfully.");
    }

    [HttpGet]
    [AuthorizationFilter(Constant.AddHomePermission)]
    [Route("{homeId}/rooms")]
    public ActionResult GetRooms([FromRoute] Guid homeId)
    {
        User currentUser = GetLoggedUser();
        List<ShowRoomDto> rooms = service.GetRooms(homeId, currentUser);

        return Ok(rooms);
    }
}
