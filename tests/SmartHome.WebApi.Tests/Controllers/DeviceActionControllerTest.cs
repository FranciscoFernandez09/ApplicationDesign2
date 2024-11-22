using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.WebApi.Controllers;

namespace SmartHome.WebApi.Tests.Controllers;

[TestClass]
public class DeviceActionControllerTest
{
    private DeviceActionController _deviceActionController = null!;
    private Mock<IDeviceActionService> _mockDeviceActionService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockDeviceActionService = new Mock<IDeviceActionService>();
        _deviceActionController = new DeviceActionController(_mockDeviceActionService.Object);
    }

    #region PersonDetectionAction

    [TestMethod]
    public void PersonDetectionAction_WhenParametersAreValid_ShouldReturnOk()
    {
        var hardwareId = Guid.NewGuid();
        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        _deviceActionController.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        _mockDeviceActionService.Setup(x => x.PersonDetectionAction(hardwareId, user));

        IActionResult result = _deviceActionController.PersonDetectionAction(hardwareId);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be("Notification of person detection sent.");
    }

    #endregion

    #region MovementDetectionAction

    [TestMethod]
    public void MovementDetectionAction_WhenParametersAreValid_ShouldReturnOk()
    {
        var hardwareId = Guid.NewGuid();
        _mockDeviceActionService.Setup(x => x.CameraMovementDetectionAction(hardwareId));

        IActionResult result = _deviceActionController.CameraMovementDetectionAction(hardwareId);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be("Notification of movement detection sent.");
    }

    #endregion

    #region OpenWindowSensor

    [TestMethod]
    public void OpenWindowSensor_WhenParametersAreValid_ShouldReturnOk()
    {
        var hardwareId = Guid.NewGuid();
        _mockDeviceActionService.Setup(x => x.ChangeWindowSensorStateTo(hardwareId, true));

        IActionResult result = _deviceActionController.OpenWindowSensor(hardwareId);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be("Notification of sensor state open sent.");
    }

    #endregion

    #region CloseWindowSensor

    [TestMethod]
    public void CloseWindowSensor_WhenParametersAreValid_ShouldReturnOk()
    {
        var hardwareId = Guid.NewGuid();
        _mockDeviceActionService.Setup(x => x.ChangeWindowSensorStateTo(hardwareId, false));

        IActionResult result = _deviceActionController.CloseWindowSensor(hardwareId);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be("Notification of sensor state close sent.");
    }

    #endregion

    #region TurnOnSmartLamp

    [TestMethod]
    public void TurnOnSmartLamp_WhenHasValidParameters_ShouldResponseOk()
    {
        var user = new User { Id = Guid.NewGuid() };
        var hardwareId = Guid.NewGuid();
        var mockHttpContext = new Mock<HttpContext>();
        _deviceActionController.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        _mockDeviceActionService.Setup(x => x.ChangeSmartLampStateTo(user, hardwareId, true));

        IActionResult result = _deviceActionController.TurnOnSmartLamp(hardwareId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Smart lamp turned on successfully.");
    }

    #endregion

    #region TurnOffSmartLamp

    [TestMethod]
    public void TurnOffSmartLamp_WhenHasValidParameters_ShouldResponseOk()
    {
        var user = new User { Id = Guid.NewGuid() };
        var hardwareId = Guid.NewGuid();
        var mockHttpContext = new Mock<HttpContext>();
        _deviceActionController.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        _mockDeviceActionService.Setup(x => x.ChangeSmartLampStateTo(user, hardwareId, false));

        IActionResult result = _deviceActionController.TurnOffSmartLamp(hardwareId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Smart lamp turned off successfully.");
    }

    #endregion

    #region MotionSensorMovementDetection

    [TestMethod]
    public void MotionSensorMovementDetection_WhenHasValidParameters_ShouldResponseOk()
    {
        var hardwareId = Guid.NewGuid();
        _mockDeviceActionService.Setup(x => x.MotionSensorMovementDetection(hardwareId));

        _deviceActionController.MotionSensorMovementDetection(hardwareId);

        _mockDeviceActionService.Verify(x => x.MotionSensorMovementDetection(hardwareId), Times.Once);
    }

    #endregion
}
