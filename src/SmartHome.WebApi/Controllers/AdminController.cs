using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Requests.Filters;

namespace SmartHome.WebApi.Controllers;

[ApiController]
[AuthenticationFilter]
public sealed class AdminController(IAdminService adminService) : SmartHomeControllerBase
{
    [HttpPost]
    [AuthorizationFilter(Constant.CreateAdmin)]
    [Route("admins")]
    public IActionResult CreateAdmin([FromBody] CreateUserRequest request)
    {
        var args = new CreateUserArgs(request.Name, request.LastName, request.Email, request.Password,
            request.ProfileImage);
        adminService.CreateAdmin(args);

        return Ok("Admin created successfully.");
    }

    [HttpDelete]
    [AuthorizationFilter(Constant.DeleteAdmin)]
    [Route("admins/{idAdmin}")]
    public IActionResult DeleteAdmin(Guid idAdmin)
    {
        adminService.DeleteAdmin(idAdmin);

        return Ok("Admin deleted successfully.");
    }

    [HttpPost]
    [AuthorizationFilter(Constant.CreateCompanyOwner)]
    [Route("companyOwners")]
    public IActionResult CreateCompanyOwner([FromBody] CreateUserRequest request)
    {
        var args = new CreateUserArgs(request.Name, request.LastName, request.Email, request.Password,
            request.ProfileImage);
        adminService.CreateCompanyOwner(args);

        return Ok("Company owner created successfully.");
    }

    [HttpGet]
    [AuthorizationFilter(Constant.GetUsers)]
    [Route("users")]
    public IActionResult GetUsers([FromQuery] FilterUserRequest request)
    {
        var dto = new FilterUserArgs(request.Name, request.LastName, request.Role, request.Offset, request.Limit);
        List<ShowUserDto> result = adminService.GetUsers(dto);

        return Ok(result);
    }

    [HttpGet]
    [AuthorizationFilter(Constant.GetCompanies)]
    [Route("companies")]
    public IActionResult GetCompanies([FromQuery] FilterCompanyRequest request)
    {
        var dto = new FilterCompanyArgs(request.CompanyName, request.OwnerName, request.OwnerLastName, request.Offset,
            request.Limit);
        List<ShowCompanyDto> result = adminService.GetCompanies(dto);

        return Ok(result);
    }

    [HttpPatch]
    [AuthorizationFilter(Constant.AdminModifyRole)]
    [Route("changeAdminRoles")]
    public IActionResult ChangeRoleToAdminHomeOwner()
    {
        User user = GetLoggedUser();
        adminService.ChangeRoleToAdminHomeOwner(user);

        return Ok("You change your role to AdminHomeOwnerRole.");
    }
}
