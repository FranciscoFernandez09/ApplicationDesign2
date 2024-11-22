using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Tests.Filters;

[TestClass]
public class AuthorizationFilterTests
{
    private AuthorizationFilterContext _context = null!;
    private DefaultHttpContext _httpContext = null!;
    private Guid _permission;

    [TestInitialize]
    public void Initialize()
    {
        _httpContext = new DefaultHttpContext();
        _context = new AuthorizationFilterContext(
            new ActionContext(
                _httpContext,
                new RouteData(),
                new ActionDescriptor()),
            new List<IFilterMetadata>());
        _permission = Guid.NewGuid();
    }

    #region Error

    [TestMethod]
    public void OnAuthorization_WhenUserIsNotAuthenticated_ShouldReturnUnauthorized()
    {
        var filter = new AuthorizationFilter(_permission.ToString());

        filter.OnAuthorization(_context);

        var result = _context.Result as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        result.Value.Should().BeEquivalentTo(new { InnerCode = "UnAuthorized", Message = "Not authenticated" });
    }

    [TestMethod]
    public void OnAuthorization_WhenUserDoesNotHavePermission_ShouldReturnForbidden()
    {
        var validArgs = new CreateUserArgs(
            "John",
            "Doe",
            "JohnDoe@gmail.com",
            "Password--12",
            string.Empty)
        { Role = new Role("User") };

        var user = new User(validArgs);
        _httpContext.Items[Item.UserLogged] = user;

        var filter = new AuthorizationFilter(_permission.ToString());

        filter.OnAuthorization(_context);

        var result = _context.Result as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        result.Value.Should()
            .BeEquivalentTo(new { InnerCode = "Forbidden", Message = $"Missing permission {_permission}" });
    }

    #endregion

    #region Success

    [TestMethod]
    public void OnAuthorization_WhenUserHasPermission_ShouldNotSetResult()
    {
        var role = new Role("Admin");
        role.Permissions.Add(new SystemPermission("Permission") { Id = _permission });

        var validArgs = new CreateUserArgs(
            "John",
            "Doe",
            "JohnDoe@gmail.com",
            "Password--12",
            string.Empty)
        { Role = role };

        var user = new User(validArgs);
        _httpContext.Items[Item.UserLogged] = user;

        var filter = new AuthorizationFilter(_permission.ToString());

        filter.OnAuthorization(_context);

        _context.Result.Should().BeNull();
    }

    [TestMethod]
    public void OnAuthorization_WhenResultIsNotNull_ShouldNotSetResult()
    {
        var filter = new AuthorizationFilter(_permission.ToString());
        _context.Result = new ObjectResult(new { InnerCode = "UnAuthorized", Message = "Not authenticated" });

        filter.OnAuthorization(_context);

        _context.Result.Should().NotBeNull();
    }

    #endregion
}
