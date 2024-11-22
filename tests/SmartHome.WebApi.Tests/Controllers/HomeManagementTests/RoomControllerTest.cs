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
public class RoomControllerTest
{
    private RoomController _controller = null!;
    private Mock<IRoomService> _service = null!;

    private User _validCurrentUser = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _service = new Mock<IRoomService>();
        _controller = new RoomController(_service.Object);

        _validCurrentUser = new User(new CreateUserArgs("John", "Doe", "john@gmail.com", "AAaa22/**/", "image.jpg")
        {
            Role = new Role("HomeOwner")
        });
    }

    #region AddRoom

    [TestMethod]
    public void AddRoom_WhenParametersAreValid_ShouldResponseOk()
    {
        var homeId = Guid.NewGuid();
        const string name = "name";
        var request = new AddRoomRequest(name);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        _service.Setup(x => x.AddAndSave(_validCurrentUser, homeId, name));

        ActionResult result = _controller.AddRoom(request, homeId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Room added successfully.");
    }

    #endregion

    #region AddDeviceToRoom

    [TestMethod]
    public void AddDeviceToRoom_WhenParametersAreValid_ShouldResponseOk()
    {
        var roomId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var request = new HardwareIdRequest(hardwareId);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        _service.Setup(x => x.AddDeviceAndSave(_validCurrentUser, roomId, hardwareId));

        ActionResult result = _controller.AddDeviceToRoom(request, roomId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Device added to room successfully.");
    }

    #endregion

    #region GetRooms

    #region Success

    [TestMethod]
    public void GetRooms_WhenCalled_ShouldReturnOkWithRooms()
    {
        var homeId = Guid.NewGuid();
        var expectedRooms = new List<ShowRoomDto> { new(Guid.NewGuid(), "Room1"), new(Guid.NewGuid(), "Room2") };

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        _service.Setup(x => x.GetRooms(homeId, _validCurrentUser)).Returns(expectedRooms);

        ActionResult result = _controller.GetRooms(homeId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeEquivalentTo(expectedRooms);
    }

    #endregion

    #endregion
}
