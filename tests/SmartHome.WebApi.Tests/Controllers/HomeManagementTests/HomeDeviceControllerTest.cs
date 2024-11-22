using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces.HomeManagement;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.WebApi.Controllers.HomeManagement;
using SmartHome.WebApi.Requests;

namespace SmartHome.WebApi.Tests.Controllers.HomeManagementTests;

[TestClass]
public class HomeDeviceControllerTest
{
    private HomeDeviceController _controller = null!;
    private Mock<IHomeDeviceService> _service = null!;

    private User _validCurrentUser = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _service = new Mock<IHomeDeviceService>();
        _controller = new HomeDeviceController(_service.Object);

        _validCurrentUser = new User(new CreateUserArgs("John", "Doe", "john@gmail.com", "AAaa22/**/", "image.jpg")
        {
            Role = new Role("HomeOwner")
        });
    }

    #region ConnectDevice

    [TestMethod]
    public void ConnectDevice_WhenHasValidParameters_ShouldResponseOk()
    {
        var hardwareId = Guid.NewGuid();

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        ActionResult result = _controller.ConnectDevice(hardwareId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Device connected successfully.");
    }

    #endregion

    #region DisconnectDevice

    [TestMethod]
    public void DisconnectDevice_WhenHasValidParameters_ShouldResponseOk()
    {
        var hardwareId = Guid.NewGuid();

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        ActionResult result = _controller.DisconnectDevice(hardwareId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Device disconnected successfully.");
    }

    #endregion

    #region UpdateHomeDeviceName

    [TestMethod]
    public void ModifyHomeDeviceName_WhenHasValidParameters_ShouldResponseOk()
    {
        var hardwareId = Guid.NewGuid();
        const string newName = "newName";

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        _service.Setup(x => x.ModifyHomeDeviceName(_validCurrentUser, hardwareId, newName));

        var request = new UpdateNameRequest(newName);

        ActionResult result = _controller.ModifyHomeDeviceName(request, hardwareId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Home device name modified successfully.");
    }

    #endregion
}
