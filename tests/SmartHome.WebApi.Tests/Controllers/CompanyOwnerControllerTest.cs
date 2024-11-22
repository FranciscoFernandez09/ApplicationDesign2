using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Requests;

namespace SmartHome.WebApi.Tests.Controllers;

[TestClass]
public class CompanyOwnerControllerTest
{
    private const string CompanyName = "Company";
    private const string CompanyRut = "0123456789-0";
    private const string CompanyLogo = "logo.png";

    private const string DeviceName = "Device";
    private const string DeviceModel = "AAA111";
    private const string DeviceDescription = "Battery";
    private readonly Guid _validatorId = Guid.Parse("f3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b");
    private Mock<ICompanyOwnerService> _companyOwnerService = null!;
    private CompanyOwnerController _controller = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _companyOwnerService = new Mock<ICompanyOwnerService>();
        _controller = new CompanyOwnerController(_companyOwnerService.Object);
    }

    #region ChangeRoleToCompanyAndHomeOwner

    [TestMethod]
    public void ChangeRoleToCompanyAndHomeOwner_WhenRoleChanged_ShouldResponseOk()
    {
        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        var items = new Dictionary<object, object> { { Item.UserLogged, user } };
        mockHttpContext.Setup(c => c.Items).Returns(items!);

        var controllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        _controller.ControllerContext = controllerContext;

        _companyOwnerService.Setup(x => x.ChangeRoleToCompanyAndHomeOwner(user));

        IActionResult result = _controller.ChangeRoleToCompanyAndHomeOwner();

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("You change your role to CompanyAndHomeOwner.");
    }

    #endregion

    #region ImportDevices

    [TestMethod]
    public void ImportDevices_WhenHasValidParameters_ShouldResponseOk()
    {
        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);
        var dllId = Guid.NewGuid();

        var request =
            new ImportDevicesRequest(new Dictionary<string, string> { { "path", "file.json" } });

        _companyOwnerService.Setup(x => x.ImportDevices(dllId, request.Parameters, user)).Verifiable();

        IActionResult result = _controller.ImportDevices(request, dllId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Devices imported successfully.");
    }

    #endregion

    #region GetDeviceImporters

    [TestMethod]
    public void GetDeviceImporters_WhenHasValidParameters_ShouldResponseOk()
    {
        var importers = new List<ShowImporterDto>
        {
            new(Guid.NewGuid(), "Importer1"), new(Guid.NewGuid(), "Importer2")
        };

        _companyOwnerService.Setup(x => x.GetDeviceImporters()).Returns(importers);

        IActionResult result = _controller.GetDeviceImporters();

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be(importers);
    }

    #endregion

    #region CreateCompany

    #region Error

    [TestMethod]
    [DataRow(null, CompanyRut, CompanyLogo, "Name")]
    [DataRow("", CompanyRut, CompanyLogo, "Name")]
    [DataRow(CompanyName, null, CompanyLogo, "Rut")]
    [DataRow(CompanyName, "", CompanyLogo, "Rut")]
    [DataRow(CompanyName, CompanyRut, null, "Logo")]
    [DataRow(CompanyName, CompanyRut, "", "Logo")]
    public void CreateCompany_WhenNullOrEmptyArgs_ShouldThrowNullException(string name, string rut, string logo,
        string invalidParamName)
    {
        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        Func<IActionResult> act = () =>
            _controller.CreateCompany(new CreateCompanyRequest(name, rut, logo, _validatorId));

        act.Should().Throw<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{invalidParamName}')");
    }

    [TestMethod]
    public void CreateCompany_WhenValidatorIdIsNull_ShouldThrowNullException()
    {
        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        Func<IActionResult> act = () =>
            _controller.CreateCompany(new CreateCompanyRequest(CompanyName, CompanyRut, CompanyLogo, null));

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'validatorId')");
    }

    [TestMethod]
    public void CreateCompany_WhenValidatorIdIsEmpty_ShouldThrowNullException()
    {
        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        Func<IActionResult> act = () =>
            _controller.CreateCompany(new CreateCompanyRequest(CompanyName, CompanyRut, CompanyLogo, Guid.Empty));

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'validatorId')");
    }

    [TestMethod]
    public void CreateCompany_WhenUserIsNull_ShouldThrowNullException()
    {
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns((User?)null);

        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        Func<IActionResult> act = () =>
            _controller.CreateCompany(new CreateCompanyRequest(CompanyName, CompanyRut, CompanyLogo, _validatorId));

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'owner')");
    }

    #endregion

    [TestMethod]
    public void CreateCompany_WhenHasValidParameters_ShouldResponseOk()
    {
        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        var request = new CreateCompanyRequest(CompanyName, CompanyRut, CompanyLogo, _validatorId);
        var args = new CreateCompanyArgs(request.Name, user, request.Rut, request.Logo, request.ValidatorId);

        _companyOwnerService.Setup(x => x.CreateCompany(args));

        IActionResult result = _controller.CreateCompany(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Company created successfully.");
    }

    #endregion

    #region CreateCamera

    #region Error

    [TestMethod]
    [DataRow(null, DeviceModel, DeviceDescription, true, true, true, true, "Name")]
    [DataRow("", DeviceModel, DeviceDescription, true, true, true, true, "Name")]
    [DataRow(DeviceName, null, DeviceDescription, true, true, true, true, "Model")]
    [DataRow(DeviceName, "", DeviceDescription, true, true, true, true, "Model")]
    [DataRow(DeviceName, DeviceModel, null, true, true, true, true, "Description")]
    [DataRow(DeviceName, DeviceModel, "", true, true, true, true, "Description")]
    [DataRow(DeviceName, DeviceModel, DeviceDescription, null, true, true, true, "hasExternalUse")]
    [DataRow(DeviceName, DeviceModel, DeviceDescription, true, null, true, true, "hasInternalUse")]
    [DataRow(DeviceName, DeviceModel, DeviceDescription, true, true, null, true, "motionDetection")]
    [DataRow(DeviceName, DeviceModel, DeviceDescription, true, true, true, null, "personDetection")]
    public void CreateCamera_WhenNullOrEmptyArgs_ShouldThrowNullException(string? name, string? model,
        string? description, bool? hasExternalUse, bool? hasInternalUse, bool? motionDetection, bool? personDetection,
        string invalidParamName)
    {
        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        var request = new CreateCameraRequest(name, model, description, hasExternalUse,
            hasInternalUse, motionDetection, personDetection, [new DeviceImageRequest("x.jpg", true)]);

        Func<IActionResult> act = () => _controller.CreateCamera(request);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{invalidParamName}')");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateCamera_WhenHasValidParameters_ShouldResponseOk()
    {
        var images = new List<DeviceImage> { new("url.png", true) };
        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        var request = new CreateCameraRequest("Camera", "AAA111", "Description", true,
            true, false, false, [new DeviceImageRequest("x.jpg", true)]);
        var args = new CreateCameraWithoutCompanyArgs(request.Name, request.Model, request.Description,
            images,
            request.HasExternalUse, request.HasInternalUse, request.MotionDetection, request.PersonDetection);

        _companyOwnerService.Setup(x => x.CreateCamera(args, user));

        IActionResult result = _controller.CreateCamera(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Camera created successfully.");
    }

    #endregion

    #endregion

    #region CreateMotionSensor

    #region Error

    [TestMethod]
    [DataRow(null, DeviceModel, DeviceDescription, "name")]
    [DataRow("", DeviceModel, DeviceDescription, "name")]
    [DataRow(DeviceName, null, DeviceDescription, "model")]
    [DataRow(DeviceName, "", DeviceDescription, "model")]
    [DataRow(DeviceName, DeviceModel, null, "description")]
    [DataRow(DeviceName, DeviceModel, "", "description")]
    public void CreateMotionSensor_WhenNullOrEmptyArgs_ShouldThrowNullException(string name, string? model,
        string description, string invalidParamName)
    {
        var request = new CreateDeviceRequest(name, model, description,
            [new DeviceImageRequest("url.png", true)]);

        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        Func<IActionResult> act = () => _controller.CreateMotionSensor(request);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{invalidParamName}')");
    }

    [TestMethod]
    public void CreateMotionSensor_WhenSmartDeviceWithoutMainImage_ShouldThrowException()
    {
        var request = new CreateDeviceRequest(DeviceName, DeviceModel, DeviceDescription,
            [new DeviceImageRequest("url.png", false)]);

        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        Func<IActionResult> act = () => _controller.CreateMotionSensor(request);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Main image is required.");
    }

    [TestMethod]
    public void CreateMotionSensor_WhenSmartDeviceWithTooManyMainImages_ShouldThrowException()
    {
        var images = new List<DeviceImageRequest> { new("url1.png", true), new("url2.jpg", true) };
        var request = new CreateDeviceRequest(DeviceName, DeviceModel, DeviceDescription, images);

        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        Func<IActionResult> act = () => _controller.CreateMotionSensor(request);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Only one main image is allowed");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateMotionSensor_WhenHasValidParameters_ShouldResponseOk()
    {
        var request = new CreateDeviceRequest(DeviceName, DeviceModel, DeviceDescription,
            [new DeviceImageRequest("url.png", true)]);

        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        var args = new CreateSmartDeviceWithoutCompanyArgs(request.Name, request.Model, request.Description,
            "MotionSensor", [new DeviceImage("url.png", true)]);

        _companyOwnerService.Setup(x => x.CreateDevice(args, user));

        IActionResult result = _controller.CreateMotionSensor(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Motion sensor created successfully.");
    }

    #endregion

    #endregion

    #region CreateWindowSensor

    #region Error

    [TestMethod]
    [DataRow(null, DeviceModel, DeviceDescription, "name")]
    [DataRow("", DeviceModel, DeviceDescription, "name")]
    [DataRow(DeviceName, null, DeviceDescription, "model")]
    [DataRow(DeviceName, "", DeviceDescription, "model")]
    [DataRow(DeviceName, DeviceModel, null, "description")]
    [DataRow(DeviceName, DeviceModel, "", "description")]
    public void CreateWindowSensor_WhenNullOrEmptyArgs_ShouldThrowNullException(string name, string? model,
        string description, string invalidParamName)
    {
        var request = new CreateDeviceRequest(name, model, description,
            [new DeviceImageRequest("url.png", true)]);

        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        Func<IActionResult> act = () => _controller.CreateWindowSensor(request);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{invalidParamName}')");
    }

    [TestMethod]
    public void CreateWindowSensor_WhenSmartDeviceWithoutMainImage_ShouldThrowException()
    {
        var request = new CreateDeviceRequest(DeviceName, DeviceModel, DeviceDescription,
            [new DeviceImageRequest("url.png", false)]);

        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        Func<IActionResult> act = () => _controller.CreateWindowSensor(request);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Main image is required.");
    }

    [TestMethod]
    public void CreateWindowSensor_WhenSmartDeviceWithTooManyMainImages_ShouldThrowException()
    {
        var images = new List<DeviceImageRequest> { new("url1.png", true), new("url2.jpg", true) };
        var request = new CreateDeviceRequest(DeviceName, DeviceModel, DeviceDescription, images);

        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        Func<IActionResult> act = () => _controller.CreateWindowSensor(request);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Only one main image is allowed");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateWindowSensor_WhenHasValidParameters_ShouldResponseOk()
    {
        var request = new CreateDeviceRequest(DeviceName, DeviceModel, DeviceDescription,
            [new DeviceImageRequest("url.png", true)]);

        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        var args = new CreateSmartDeviceWithoutCompanyArgs(request.Name, request.Model, request.Description,
            "WindowSensor",
            [new DeviceImage("url.png", true)]);

        _companyOwnerService.Setup(x => x.CreateDevice(args, user));

        IActionResult result = _controller.CreateWindowSensor(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Window sensor created successfully.");
    }

    #endregion

    #endregion

    #region CreateSmartLamp

    #region Error

    [TestMethod]
    [DataRow(null, DeviceModel, DeviceDescription, "name")]
    [DataRow("", DeviceModel, DeviceDescription, "name")]
    [DataRow(DeviceName, null, DeviceDescription, "model")]
    [DataRow(DeviceName, "", DeviceDescription, "model")]
    [DataRow(DeviceName, DeviceModel, null, "description")]
    [DataRow(DeviceName, DeviceModel, "", "description")]
    public void CreateSmartLamp_WhenNullOrEmptyArgs_ShouldThrowNullException(string name, string? model,
        string description, string invalidParamName)
    {
        var request = new CreateDeviceRequest(name, model, description,
            [new DeviceImageRequest("url.png", true)]);

        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        Func<IActionResult> act = () => _controller.CreateSmartLamp(request);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{invalidParamName}')");
    }

    [TestMethod]
    public void CreateSmartLamp_WhenSmartDeviceWithoutMainImage_ShouldThrowException()
    {
        var request = new CreateDeviceRequest(DeviceName, DeviceModel, DeviceDescription,
            [new DeviceImageRequest("url.png", false)]);

        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        Func<IActionResult> act = () => _controller.CreateSmartLamp(request);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Main image is required.");
    }

    [TestMethod]
    public void CreateSmartLamp_WhenSmartDeviceWithTooManyMainImages_ShouldThrowException()
    {
        var images = new List<DeviceImageRequest> { new("url1.jpg", true), new("url2.jpg", true) };
        var request = new CreateDeviceRequest(DeviceName, DeviceModel, DeviceDescription, images);

        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        Func<IActionResult> act = () => _controller.CreateSmartLamp(request);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Only one main image is allowed");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateSmartLamp_WhenHasValidParameters_ShouldResponseOk()
    {
        var request = new CreateDeviceRequest(DeviceName, DeviceModel, DeviceDescription,
            [new DeviceImageRequest("url.png", true)]);

        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(user);

        var args = new CreateSmartDeviceWithoutCompanyArgs(request.Name, request.Model, request.Description,
            "SmartLamp",
            [new DeviceImage("url.png", true)]);

        _companyOwnerService.Setup(x => x.CreateDevice(args, user));

        IActionResult result = _controller.CreateSmartLamp(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Smart lamp created successfully.");
    }

    #endregion

    #region GetImplementations

    [TestMethod]
    public void GetImplementations_WhenHasValidParameters_ShouldResponseOk()
    {
        var expectedValidators =
            new List<ShowModelValidatorsDto> { new(Guid.NewGuid(), "Validator1"), new(Guid.NewGuid(), "Validator2") };

        _companyOwnerService.Setup(x => x.GetModelValidators()).Returns(expectedValidators);

        IActionResult result = _controller.GetModelValidators();

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be(expectedValidators);
    }

    #endregion

    #endregion
}
