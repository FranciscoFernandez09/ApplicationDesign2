using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Tests.Filters;

[TestClass]
public class AuthenticationFilterTests
{
    private readonly AuthenticationFilterAttribute _authenticationFilter = new();
    private AuthorizationFilterContext _context = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private Mock<ISessionService> _sessionServiceMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _sessionServiceMock = new Mock<ISessionService>(MockBehavior.Strict);

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(sp => sp.GetService(typeof(ISessionService))).Returns(_sessionServiceMock.Object);

        _httpContextMock.Setup(h => h.RequestServices).Returns(serviceProviderMock.Object);

        _context = new AuthorizationFilterContext(
            new ActionContext(
                _httpContextMock.Object,
                new RouteData(),
                new ActionDescriptor()),
            new List<IFilterMetadata>());
    }

    #region Success

    [TestMethod]
    public void OnAuthorization_WhenSessionIsValid_ShouldNotSetResult()
    {
        var validToken = Guid.NewGuid();

        _httpContextMock.Setup(h => h.Request.Headers).Returns(
            new HeaderDictionary(new Dictionary<string, StringValues> { { "Authorization", validToken.ToString() } }));
        _sessionServiceMock.Setup(s => s.IsValidSession(validToken)).Returns(true);
        _sessionServiceMock.Setup(s => s.GetUserByToken(validToken)).Returns(new User());

        var contextItems = new Dictionary<object, object>();
        _httpContextMock.Setup(h => h.Items).Returns(contextItems!);

        _authenticationFilter.OnAuthorization(_context);

        _httpContextMock.VerifyAll();
        _sessionServiceMock.VerifyAll();
        _context.Result.Should().BeNull();
    }

    #endregion

    private static string? GetInnerCode(object value)
    {
        return value.GetType().GetProperty("InnerCode")?.GetValue(value)?.ToString();
    }

    private static string? GetMessage(object value)
    {
        return value.GetType().GetProperty("Message")?.GetValue(value)?.ToString();
    }

    #region Error

    [TestMethod]
    public void OnAuthorization_WhenEmptyHeaders_ShouldReturnUnauthenticatedResponse()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary());

        _authenticationFilter.OnAuthorization(_context);

        IActionResult? response = _context.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse?.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        if (concreteResponse?.Value == null)
        {
            return;
        }

        GetInnerCode(concreteResponse.Value).Should().Be("Unauthenticated");
        GetMessage(concreteResponse.Value).Should().Be("You are not authenticated");
    }

    [TestMethod]
    public void OnAuthorization_WhenAuthorizationIsEmpty_ShouldReturnUnauthenticatedResponse()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Returns(
            new HeaderDictionary(new Dictionary<string, StringValues> { { "Authorization", string.Empty } }));

        _authenticationFilter.OnAuthorization(_context);

        IActionResult? response = _context.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse?.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        if (concreteResponse?.Value == null)
        {
            return;
        }

        GetInnerCode(concreteResponse.Value).Should().Be("Unauthenticated");
        GetMessage(concreteResponse.Value).Should().Be("You are not authenticated");
    }

    [TestMethod]
    public void OnAuthorization_WhenAuthorizationIsInvalid_ShouldReturnInvalidTokenResponse()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Returns(
            new HeaderDictionary(new Dictionary<string, StringValues> { { "Authorization", "invalid-token" } }));

        _authenticationFilter.OnAuthorization(_context);

        IActionResult? response = _context.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse?.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        if (concreteResponse?.Value == null)
        {
            return;
        }

        GetInnerCode(concreteResponse.Value).Should().Be("Invalid authorization token");
        GetMessage(concreteResponse.Value).Should().Be("Invalid authorization token format");
    }

    [TestMethod]
    public void OnAuthorization_WhenSessionIsNotValid_ShouldReturnNotValidSessionResponse()
    {
        var validToken = Guid.NewGuid().ToString();
        _httpContextMock.Setup(h => h.Request.Headers).Returns(
            new HeaderDictionary(new Dictionary<string, StringValues> { { "Authorization", validToken } }));
        _sessionServiceMock.Setup(s => s.IsValidSession(It.IsAny<Guid>())).Returns(false);

        _authenticationFilter.OnAuthorization(_context);

        IActionResult? response = _context.Result;

        _httpContextMock.VerifyAll();
        _sessionServiceMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse?.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        if (concreteResponse?.Value == null)
        {
            return;
        }

        GetInnerCode(concreteResponse.Value).Should().Be("Not valid session");
        GetMessage(concreteResponse.Value).Should().Be("The token does not correspond to a session");
    }

    [TestMethod]
    public void OnAuthorization_WhenSessionIsNotValid_ShouldReturnInvalidSessionResponse()
    {
        var validToken = Guid.NewGuid().ToString();
        _httpContextMock.Setup(h => h.Request.Headers).Returns(
            new HeaderDictionary(new Dictionary<string, StringValues> { { "Authorization", validToken } }));
        _sessionServiceMock.Setup(s => s.IsValidSession(It.IsAny<Guid>())).Returns(true);
        _sessionServiceMock.Setup(s => s.GetUserByToken(It.IsAny<Guid>())).Throws<Exception>();

        _authenticationFilter.OnAuthorization(_context);

        IActionResult? response = _context.Result;

        _httpContextMock.VerifyAll();
        _sessionServiceMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse?.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);

        GetInnerCode(concreteResponse!.Value!).Should().Be("Invalid Session");
        GetMessage(concreteResponse.Value!).Should().Be("The token does not correspond to a session");
    }

    #endregion
}
