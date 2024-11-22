using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthorizationFilter(string permission) : Attribute, IAuthorizationFilter
{
    private readonly Guid _permission = Guid.Parse(permission);

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.Result != null)
        {
            return;
        }

        var userLogged = context.HttpContext.Items[Item.UserLogged];

        var userIsNotIdentified = userLogged == null;
        if (userIsNotIdentified)
        {
            context.Result =
                new ObjectResult(new { InnerCode = "UnAuthorized", Message = "Not authenticated" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            return;
        }

        var userLoggedMapped = (User)userLogged!;

        var hasNotPermission = !userLoggedMapped.RoleHasRequiredSystemPermission(_permission);

        if (hasNotPermission)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Forbidden",
                Message = $"Missing permission {_permission}"
            })
            { StatusCode = StatusCodes.Status403Forbidden };
        }
    }
}
