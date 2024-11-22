using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.WebApi.Requests;

namespace SmartHome.WebApi.Controllers;

[ApiController]
public class UnknownUserController(IUnknownUserService unknownUserService) : SmartHomeControllerBase
{
    [HttpPost]
    [Route("homeOwners")]
    public IActionResult CreateHomeOwner([FromBody] CreateUserRequest request)
    {
        var args = new CreateUserArgs(request.Name, request.LastName, request.Email, request.Password,
            request.ProfileImage);
        unknownUserService.CreateHomeOwner(args);
        return Ok("User created successfully.");
    }
}
