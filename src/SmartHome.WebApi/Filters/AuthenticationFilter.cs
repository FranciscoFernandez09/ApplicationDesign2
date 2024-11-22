using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthenticationFilterAttribute
    : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        StringValues authorizationHeader = context.HttpContext.Request.Headers[HeaderNames.Authorization];
        ISessionService sessionService = context.HttpContext.RequestServices.GetRequiredService<ISessionService>();
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Unauthenticated",
                Message = "You are not authenticated"
            })
            { StatusCode = (int)HttpStatusCode.Unauthorized };
            return;
        }

        if (!Guid.TryParse(authorizationHeader, out Guid authHeaderGuid))
        {
            context.Result =
                new ObjectResult(new
                {
                    InnerCode = "Invalid authorization token",
                    Message = "Invalid authorization token format"
                })
                { StatusCode = (int)HttpStatusCode.Unauthorized };
            return;
        }

        var isSessionValid = sessionService.IsValidSession(authHeaderGuid);
        if (!isSessionValid)
        {
            context.Result =
                new ObjectResult(new
                {
                    InnerCode = "Not valid session",
                    Message = "The token does not correspond to a session"
                })
                { StatusCode = (int)HttpStatusCode.Unauthorized };
            return;
        }

        try
        {
            User userOfAuthorization = sessionService.GetUserByToken(authHeaderGuid);
            context.HttpContext.Items[Item.UserLogged] = userOfAuthorization;
        }
        catch (Exception)
        {
            context.Result =
                new ObjectResult(new
                {
                    InnerCode = "Invalid Session",
                    Message = "The token does not correspond to a session"
                })
                { StatusCode = (int)HttpStatusCode.Unauthorized };
        }
    }
}
