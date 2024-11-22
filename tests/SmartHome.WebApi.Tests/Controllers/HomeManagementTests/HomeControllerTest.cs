using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces.HomeManagement;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.WebApi.Controllers.HomeManagement;
using SmartHome.WebApi.Requests;

namespace SmartHome.WebApi.Tests.Controllers.HomeManagementTests;

[TestClass]
public class HomeControllerTest
{
    private const string AddressStreet = "street";
    private const int AddressNumber = 100;
    private const int Latitude = 100;
    private const int Longitude = 100;
    private const int MaxMembers = 5;
    private const string Name = "name";
    private HomeController _controller = null!;
    private Mock<IHomeService> _service = null!;

    private User _validCurrentUser = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _service = new Mock<IHomeService>();
        _controller = new HomeController(_service.Object);

        _validCurrentUser = new User(new CreateUserArgs("John", "Doe", "john@gmail.com", "AAaa22/**/", "image.jpg")
        {
            Role = new Role("HomeOwner")
        });
    }

    #region AddMember

    [TestMethod]
    public void AddMember_WhenParametersAreValid_ShouldResponseOk()
    {
        var memberEmail = _validCurrentUser.Email;
        var homeId = Guid.NewGuid();

        var request = new AddMemberRequest(memberEmail);
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        ActionResult result = _controller.AddMember(request, homeId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Home member added successfully.");
    }

    #endregion

    #region AddSmartDevice

    [TestMethod]
    public void AddSmartDevice_WhenParametersAreValid_ShouldResponseOk()
    {
        var deviceId = Guid.NewGuid();
        var homeId = Guid.NewGuid();

        var request = new AddDeviceRequest(deviceId);
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        ActionResult result = _controller.AddSmartDevice(request, homeId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Smart device added successfully.");
    }

    #endregion

    #region GetHomeMembers

    [TestMethod]
    public void GetHomeMembers_WhenParametersValid_ShouldResponseOk()
    {
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        var homeId = Guid.NewGuid();
        var members =
            new List<ShowHomeMemberDto> { new(Guid.NewGuid(), "name", "lastName", "image", true, "permission") };

        _service.Setup(x => x.GetHomeMembers(_validCurrentUser, homeId)).Returns(members);

        ActionResult result = _controller.GetHomeMembers(homeId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeEquivalentTo(members);
    }

    #endregion

    #region GetHomeDevices

    [TestMethod]
    public void GetHomeDevices_WhenParametersValid_ShouldResponseOk()
    {
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        var homeId = Guid.NewGuid();
        Guid? roomId = Guid.NewGuid();
        var homeDevices = new List<ShowHomeDeviceDto> { new(homeId, "name", "Camera", true, "AAA111", "image", true) };

        _service.Setup(x => x.GetHomeDevices(_validCurrentUser, homeId, roomId)).Returns(homeDevices);

        ActionResult result = _controller.GetHomeDevices(homeId, roomId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeEquivalentTo(homeDevices);
    }

    #endregion

    #region ModifyHomeName

    [TestMethod]
    public void ModifyHomeName_WhenHasValidParameters_ShouldResponseOk()
    {
        var homeId = Guid.NewGuid();
        const string newName = "newName";

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        var request = new UpdateNameRequest(newName);

        ActionResult result = _controller.ModifyHomeName(request, homeId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Home name modified successfully.");
    }

    #endregion

    #region GetPermissions

    [TestMethod]
    public void GetPermissions_WhenCalled_ShouldReturnOk()
    {
        var permissions = new List<ShowHomePermissionDto> { new(Guid.NewGuid(), "PermissionName") };

        _service.Setup(x => x.GetHomePermissions()).Returns(permissions);

        ActionResult result = _controller.GetPermissions();

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeEquivalentTo(permissions);
    }

    #endregion

    #region CreateHome

    #region Error

    [TestMethod]
    [DataRow(null, AddressStreet, AddressNumber, Latitude, Longitude, MaxMembers, "name")]
    [DataRow("", AddressStreet, AddressNumber, Latitude, Longitude, MaxMembers, "name")]
    [DataRow(Name, null, AddressNumber, Latitude, Longitude, MaxMembers, "addressStreet")]
    [DataRow(Name, "", AddressNumber, Latitude, Longitude, MaxMembers, "addressStreet")]
    [DataRow(Name, AddressStreet, null, Latitude, Longitude, MaxMembers, "addressNumber")]
    [DataRow(Name, AddressStreet, AddressNumber, null, Longitude, MaxMembers, "latitude")]
    [DataRow(Name, AddressStreet, AddressNumber, Latitude, null, MaxMembers, "longitude")]
    [DataRow(Name, AddressStreet, AddressNumber, Latitude, Longitude, null, "maxMembers")]
    public void CreateHome_WhenParametersAreNullOrEmpty_ShouldThrowArgumentNullException(string? name,
        string? addressStreet, int? addressNumber, int? latitude, int? longitude, int? maxMembers,
        string invalidParamName)
    {
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        var request = new CreateHomeRequest(name, addressStreet, addressNumber, latitude, longitude, maxMembers);

        Func<ActionResult> act = () => _controller.CreateHome(request);

        act.Should().ThrowExactly<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{invalidParamName}')");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateHome_WhenParametersAreValid_ShouldResponseOk()
    {
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        var request = new CreateHomeRequest(Name, AddressStreet, AddressNumber, Latitude,
            Longitude, MaxMembers);

        ActionResult result = _controller.CreateHome(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Home created successfully.");
    }

    #endregion

    #endregion

    #region AddHomePermission

    #region Error

    [TestMethod]
    public void AddHomePermission_WhenPermissionIdIsNull_ShouldThrowArgumentNullException()
    {
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        var request = new AddHomePermissionRequest(null);

        Func<ActionResult> act = () => _controller.AddHomePermission(request, Guid.NewGuid());

        act.Should().ThrowExactly<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'permissionId')");
    }

    #endregion

    #region Success

    [TestMethod]
    public void AddHomePermission_WhenParametersAreValid_ShouldResponseOk()
    {
        var memberId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        var request = new AddHomePermissionRequest(permissionId);

        ActionResult result = _controller.AddHomePermission(request, memberId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Home permission added successfully.");
    }

    #endregion

    #endregion

    #region GetMineHomes

    [TestMethod]
    public void GetHomes_WhenUserDoesntHaveHomes_ShouldReturnEmptyAndBeOk()
    {
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        _service.Setup(x => x.GetMineHomes(_validCurrentUser)).Returns([]);

        ActionResult result = _controller.GetMineHomes();

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;

        okObjectResult?.Value.Should().BeEquivalentTo(new List<ShowHomeDto>());
    }

    [TestMethod]
    public void GetHomes_WhenUserHasHomes_ShouldReturnHomesAndBeOk()
    {
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        var homeDto = new ShowHomeDto(Guid.NewGuid(), "name");

        var homes = new List<ShowHomeDto> { homeDto };
        _service.Setup(x => x.GetMineHomes(_validCurrentUser)).Returns(homes);

        ActionResult result = _controller.GetMineHomes();

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeEquivalentTo(homes);
    }

    #endregion

    #region GetHomesWhereIMember

    [TestMethod]
    public void GetHomesWhereIMember_WhenUserDoesntHaveHomes_ShouldReturnEmptyAndBeOk()
    {
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        _service.Setup(x => x.GetHomesWhereIMember(_validCurrentUser)).Returns([]);

        ActionResult result = _controller.GetHomesWhereIMember();

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;

        okObjectResult?.Value.Should().BeEquivalentTo(new List<ShowHomeDto>());
    }

    [TestMethod]
    public void GetHomesWhereIMember_WhenUserHasHomes_ShouldReturnHomesAndBeOk()
    {
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        var homeDto = new ShowHomeDto(Guid.NewGuid(), "name");

        var homes = new List<ShowHomeDto> { homeDto };
        _service.Setup(x => x.GetHomesWhereIMember(_validCurrentUser)).Returns(homes);

        ActionResult result = _controller.GetHomesWhereIMember();

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeEquivalentTo(homes);
    }

    #endregion
}
