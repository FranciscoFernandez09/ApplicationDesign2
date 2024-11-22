using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Requests;

namespace SmartHome.WebApi.Tests.Controllers;

[TestClass]
public class SessionControllerTest
{
    private SessionController _controller = null!;
    private Mock<ISessionService> _sessionServiceMock = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _sessionServiceMock = new Mock<ISessionService>();

        _controller = new SessionController(_sessionServiceMock.Object);
    }

    #region Login

    [TestMethod]
    public void Login_WhenUserExists_ShouldReturnOk()
    {
        var dto = new SessionDto(Guid.NewGuid(), Guid.NewGuid(), "name");
        _sessionServiceMock.Setup(x => x.Login("email", "Password1!")).Returns(dto);

        var request = new LoginRequest("email", "Password1!");

        IActionResult result = _controller.Login(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeEquivalentTo(dto);
    }

    #endregion

    #region Logout

    [TestMethod]
    public void Logout_WhenParametersAreValid_ShouldReturnOk()
    {
        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        _sessionServiceMock.Setup(x => x.Logout(It.IsAny<User>()));
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        IActionResult result = _controller.Logout();

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("User logged out successfully.");
    }

    #endregion
}
