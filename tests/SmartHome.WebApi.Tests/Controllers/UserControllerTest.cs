using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Requests.Filters;

namespace SmartHome.WebApi.Tests.Controllers;

[TestClass]
public class UserControllerTest
{
    private const string DeviceName = "Device";
    private const string DeviceModel = "AAA111";
    private const string CompanyName = "Company";
    private const string DeviceType = "DeviceType";

    private const int Offset = 0;
    private const int Limit = 10;
    private static readonly Guid Guid = default;
    private UserController _controller = null!;

    private Mock<IUserService> _serviceMock = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _serviceMock = new Mock<IUserService>();
        _controller = new UserController(_serviceMock.Object);
    }

    #region ModifyProfileImage

    [TestMethod]
    public void ModifyProfileImage_WhenParametersAreValid_ShouldReturnOk()
    {
        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        var items = new Dictionary<object, object> { { Item.UserLogged, user } };
        mockHttpContext.Setup(c => c.Items).Returns(items!);

        var controllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        _controller.ControllerContext = controllerContext;

        var request = new UpdateProfileImageRequest("ProfileImage");

        IActionResult result = _controller.ModifyProfileImage(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Profile image modified successfully.");
    }

    #endregion

    #region GetDevices

    #region Error

    [TestMethod]
    [DataRow(null, 1, "offset")]
    [DataRow(1, null, "limit")]
    public void GetDevices_WhenOffsetOrLimitIsNull_ShouldThrowArgumentNullException(int? offset, int? limit,
        string invalidParamName)
    {
        var request = new FilterDeviceRequest(DeviceName, DeviceModel, CompanyName, DeviceType, offset, limit);

        Func<IActionResult> act = () => _controller.GetDevices(request);

        act.Should().ThrowExactly<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{invalidParamName}')");
    }

    [TestMethod]
    public void GetDevices_WhenOffLowerThanZero_ShouldThrowArgumentException()
    {
        var request = new FilterDeviceRequest(DeviceName, DeviceModel, CompanyName, DeviceType, -1, Limit);

        Func<IActionResult> act = () => _controller.GetDevices(request);

        act.Should().ThrowExactly<ArgumentException>().WithMessage("Offset must be greater or equal than 0.");
    }

    [TestMethod]
    public void GetDevices_WhenLimitIsNegative_ShouldThrowArgumentException()
    {
        var request = new FilterDeviceRequest(DeviceName, DeviceModel, CompanyName, DeviceType, Offset, 0);

        Func<IActionResult> act = () => _controller.GetDevices(request);

        act.Should().ThrowExactly<ArgumentException>().WithMessage("Limit must be greater than 0.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetDevices_WhenParametersAreValid_ShouldReturnOk()
    {
        var devices = new List<ShowDeviceDto>();
        _serviceMock.Setup(x => x.GetDevices(It.IsAny<FilterDeviceArgs>())).Returns(devices);
        var request = new FilterDeviceRequest(DeviceName, DeviceModel, CompanyName, DeviceType, Offset, Limit);

        IActionResult result = _controller.GetDevices(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeEquivalentTo(devices);
    }

    [TestMethod]
    public void GetDevices_WhenParametersAreValid_ShouldReturnOkWithItems()
    {
        var devices = new List<ShowDeviceDto> { new(Guid, DeviceName, DeviceModel, "ImageUrl", CompanyName) };

        _serviceMock.Setup(x => x.GetDevices(It.IsAny<FilterDeviceArgs>())).Returns(devices);

        var request = new FilterDeviceRequest(DeviceName, DeviceModel, CompanyName, DeviceType, Offset, Limit);
        IActionResult result = _controller.GetDevices(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeEquivalentTo(devices);
    }

    #endregion

    #endregion

    #region GetDevicesTypes

    [TestMethod]
    public void GetDeviceTypes_WhenParametersAreValid_ShouldReturnOk()
    {
        _serviceMock.Setup(x => x.GetDevicesTypes()).Returns([]);

        IActionResult result = _controller.GetDevicesTypes();

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeOfType<List<string>>();
    }

    [TestMethod]
    public void GetDeviceTypes_WhenParametersAreValid_ShouldReturnOkWithItems()
    {
        var deviceTypes = new List<string> { "Sensor", "Camera" };

        _serviceMock.Setup(x => x.GetDevicesTypes()).Returns(deviceTypes);

        IActionResult result = _controller.GetDevicesTypes();

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeEquivalentTo(deviceTypes);
    }

    #endregion
}
