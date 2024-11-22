using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.Controllers;

public class SmartHomeControllerBase : ControllerBase
{
    protected User GetLoggedUser()
    {
        var userLogged = HttpContext.Items[Item.UserLogged];
        var userLoggedMapped = (User)userLogged!;
        return userLoggedMapped;
    }
}
