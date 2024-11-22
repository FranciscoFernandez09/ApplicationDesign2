using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces.HomeManagement;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.WebApi.Controllers.HomeManagement;
using SmartHome.WebApi.Requests.Filters;

namespace SmartHome.WebApi.Tests.Controllers.HomeManagementTests;

[TestClass]
public class MemberControllerTest
{
    private MemberController _controller = null!;
    private Mock<IMemberService> _service = null!;

    private User _validCurrentUser = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _service = new Mock<IMemberService>();
        _controller = new MemberController(_service.Object);

        _validCurrentUser = new User(new CreateUserArgs("John", "Doe", "john@gmail.com", "AAaa22/**/", "image.jpg")
        {
            Role = new Role("HomeOwner")
        });
    }

    #region ActivateMemberNotification

    [TestMethod]
    public void ActivateMemberNotification_WhenHasValidParameters_ShouldResponseOk()
    {
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        Guid? memberId = Guid.NewGuid();

        ActionResult result = _controller.ActivateMemberNotification(memberId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Member notification activated successfully.");
    }

    #endregion

    #region DesactivateMemberNotification

    [TestMethod]
    public void DeactivateMemberNotification_WhenHasValidParameters_ShouldResponseOk()
    {
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        Guid? memberId = Guid.NewGuid();

        ActionResult result = _controller.DeactivateMemberNotification(memberId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Member notification deactivated successfully.");
    }

    #endregion

    #region GetNotifications

    [TestMethod]
    public void GetNotifications_WhenHasEmptyList_ShouldResponseOk()
    {
        var time = DateTime.Now.ToString("yyyy-MM-dd");
        const string? deviceType = "camera";
        bool? isRead = null;

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        List<ShowNotificationDto> expected = [];

        // var dto = new FilterNotificationsDto(_validCurrentUser, deviceType, time, isRead);
        // _homeOwnerService.Setup(x => x.GetNotifications(dto)).Returns(expected);
        _service.Setup(x => x.GetNotifications(It.IsAny<FilterNotificationsArgs>())).Returns(expected);

        var request = new FilterNotificationRequest(deviceType, time, isRead);
        ActionResult result = _controller.GetNotifications(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeEquivalentTo(new List<ShowUserDto>());
    }

    [TestMethod]
    public void GetNotifications_WhenHasList_ShouldResponseOk()
    {
        const string? deviceType = null;
        bool? isRead = null;

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Items[Item.UserLogged]).Returns(_validCurrentUser);
        _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
        var expected = new List<ShowNotificationDto>
        {
            new(_validCurrentUser.Id, "Detected", Guid.NewGuid(), true, DateTime.Now)
        };

        // var dto = new FilterNotificationsDto(_validCurrentUser, deviceType, time, isRead);
        // _homeOwnerService.Setup(x => x.GetNotifications(dto)).Returns(expected);
        _service.Setup(x => x.GetNotifications(It.IsAny<FilterNotificationsArgs>())).Returns(expected);

        var request = new FilterNotificationRequest(deviceType, null, isRead);
        ActionResult result = _controller.GetNotifications(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeEquivalentTo(expected);
    }

    #endregion
}
