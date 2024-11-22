using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using SmartHome.BusinessLogic.AssemblyManagement;
using SmartHome.DataAccess;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Tests.Filters;

[TestClass]
public class ExceptionFilterTest
{
    private readonly ExceptionFilter _attribute = new();
    private ExceptionContext _context = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context = new ExceptionContext(
            new ActionContext(new Mock<HttpContext>().Object, new RouteData(), new ActionDescriptor()),
            new List<IFilterMetadata>());
    }

    [TestMethod]
    public void OnException_WhenExceptionIsNotRegistered_ShouldResponseInternalError()
    {
        _context.Exception = new Exception("Not registered");

        _attribute.OnException(_context);

        IActionResult? response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        GetInnerCode(concreteResponse.Value!).Should().Be("InternalServerError");
        GetMessage(concreteResponse.Value!).Should().Be("There was an error when processing the request");
    }

    [TestMethod]
    public void OnException_WhenArgumentNullException_ShouldResponseBadRequest()
    {
        const string paramName = "paramName";
        _context.Exception = new ArgumentNullException(paramName, "Argument cannot be null or empty");

        _attribute.OnException(_context);

        IActionResult? response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        GetInnerCode(concreteResponse.Value!).Should().Be("ArgumentNull");
        GetMessage(concreteResponse.Value!).Should().Be("Argument cannot be null or empty");
    }

    [TestMethod]
    public void OnException_WhenInvalidOperationException_ShouldResponseBadRequest()
    {
        _context.Exception = new InvalidOperationException("Cannot continue");

        _attribute.OnException(_context);

        IActionResult? response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        GetInnerCode(concreteResponse.Value!).Should().Be("InvalidOperation");
        GetMessage(concreteResponse.Value!).Should().Be("Cannot continue");
    }

    [TestMethod]
    public void OnException_WhenUnauthorizedAccessException_ShouldResponseUnauthorized()
    {
        _context.Exception = new UnauthorizedAccessException("Unauthorized access");

        _attribute.OnException(_context);

        IActionResult? response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        GetInnerCode(concreteResponse.Value!).Should().Be("UnauthorizedAccess");
        GetMessage(concreteResponse.Value!).Should().Be("Unauthorized access");
    }

    [TestMethod]
    public void OnException_WhenArgumentException_ShouldResponseBadRequest()
    {
        _context.Exception = new ArgumentException("Invalid argument");

        _attribute.OnException(_context);

        IActionResult? response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        GetInnerCode(concreteResponse.Value!).Should().Be("InvalidArgument");
        GetMessage(concreteResponse.Value!).Should().Be("Invalid argument");
    }

    [TestMethod]
    public void OnException_WhenDataAccessException_ShouldResponseInternalServerError()
    {
        _context.Exception = new DataAccessException("Error accessing data");

        _attribute.OnException(_context);

        IActionResult? response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        GetInnerCode(concreteResponse.Value!).Should().Be("DataAccessError");
        GetMessage(concreteResponse.Value!).Should().Be("Error accessing data");
    }

    [TestMethod]
    public void OnException_WhenAssemblyException_ShouldResponseInternalServerError()
    {
        _context.Exception = new AssemblyException("Error loading assembly");

        _attribute.OnException(_context);

        IActionResult? response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        GetInnerCode(concreteResponse.Value!).Should().Be("AssemblyError");
        GetMessage(concreteResponse.Value!).Should().Be("Error loading assembly");
    }

    private string GetInnerCode(object value)
    {
        return value.GetType().GetProperty("InnerCode")!.GetValue(value)!.ToString()!;
    }

    private string GetMessage(object value)
    {
        return value.GetType().GetProperty("Message")!.GetValue(value)!.ToString()!;
    }
}
