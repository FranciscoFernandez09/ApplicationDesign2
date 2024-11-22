using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;

namespace SmartHome.WebApi.Controllers;

[Route("sessions")]
public sealed class SessionController(ISessionService sessionService) : SmartHomeControllerBase
{
    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        SessionDto response = sessionService.Login(request.Email, request.Password);
        return Ok(response);
    }

    [HttpDelete]
    [AuthenticationFilter]
    public IActionResult Logout()
    {
        User user = GetLoggedUser();
        sessionService.Logout(user);

        return Ok("User logged out successfully.");
    }
}
