using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.BusinessLogic.Services.HomeManagement;

namespace SmartHome.BusinessLogic.Tests.ServicesTests.HomeManagementTests;

[TestClass]
public class MemberServiceTest
{
    private const bool State = true;
    private Mock<IRepository<HomeMember>> _homeMemberRepositoryMock = null!;
    private Mock<IRepository<Notification>> _notificationRepositoryMock = null!;

    private MemberService _service = null!;
    private Company _validCompany = null!;
    private User _validCurrentUser = null!;
    private Home _validHome = null!;
    private SmartDevice _validSmartDevice = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        const string name = "Alan";
        const string lastName = "Smith";
        const string email = "alan@gmail.com";
        const string password = "Password-123";
        const string profileImage = "";

        const string addressStreet = "ValidAddressStreet";
        const int addressNumber = 123;
        const int latitude = 10;
        const int longitude = 99;
        const int maxMembers = 4;
        const string homeName = "name";
        const string validDeviceType = "MotionSensor";

        _homeMemberRepositoryMock = new Mock<IRepository<HomeMember>>(MockBehavior.Strict);
        _notificationRepositoryMock = new Mock<IRepository<Notification>>(MockBehavior.Strict);

        _service = new MemberService(_homeMemberRepositoryMock.Object, _notificationRepositoryMock.Object);

        _validCurrentUser =
            new User(new CreateUserArgs(name, lastName, email, password, profileImage)
            {
                Role = new Role { Id = Constant.HomeOwnerRoleId, Name = "HomeOwner" }
            });

        _validHome = new Home(new CreateHomeArgs(addressStreet, addressNumber, latitude, longitude,
            maxMembers, homeName, _validCurrentUser));

        var imageList = new List<DeviceImage> { new("logo.png", true) };
        _validCompany = new Company(new CreateCompanyArgs("company", _validCurrentUser, "1234567890-1", "logo.png",
            Guid.Parse("f3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b")));
        _validSmartDevice =
            new SmartDevice(
                new CreateSmartDeviceArgs("Device", "AAA111", "Description", _validCompany, validDeviceType,
                    imageList));
    }

    #region DeactivateMemberToNotification

    #region Error

    [TestMethod]
    public void ChangeMemberNotificationStateTo_WhenMemberIdIsNull_ShouldThrowArgumentNullException()
    {
        Action act = () => _service.ChangeMemberNotificationStateTo(_validCurrentUser, null, State);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'memberId')");
    }

    [TestMethod]
    public void ChangeMemberNotificationStateTo_WhenMemberIdIsEmpty_ShouldThrowArgumentNullException()
    {
        Action act = () => _service.ChangeMemberNotificationStateTo(_validCurrentUser, Guid.Empty, State);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'memberId')");
    }

    [TestMethod]
    public void ChangeMemberNotificationStateTo_WhenMemberIsNotFound_ShouldThrowInvalidOperationException()
    {
        Guid? memberId = _validCurrentUser.Id;

        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.Id == memberId)).Returns((HomeMember?)null);

        Action act = () => _service.ChangeMemberNotificationStateTo(_validCurrentUser, memberId, State);

        act.Should().Throw<InvalidOperationException>().WithMessage("Member not found in the home.");
    }

    [TestMethod]
    public void ChangeMemberNotificationStateTo_WhenMemberIsNotHomeOwner_ShouldThrowUnAuthorizedException()
    {
        var user =
            new User(new CreateUserArgs("John", "Doe", "john@gmail.com", "Password-123", string.Empty)
            {
                Role = new Role("HomeOwner")
            });
        var member = new HomeMember(_validHome, user);
        Guid? memberId = member.Id;

        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.Id == memberId)).Returns(member);
        Action act = () => _service.ChangeMemberNotificationStateTo(user, memberId, State);

        act.Should().Throw<UnauthorizedAccessException>()
            .WithMessage("User does not have permission to modify member notification privileges.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void
        ChangeMemberNotificationStateTo_WhenParametersAreValidAndStateIsTrue_ShouldChangeMemberNotificationStateToTrue()
    {
        var member = new HomeMember(_validHome, _validCurrentUser);

        Guid? memberId = _validCurrentUser.Id;

        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.Id == memberId)).Returns(member);
        _homeMemberRepositoryMock.Setup(ns => ns.Update(It.IsAny<HomeMember>())).Verifiable();

        _service.ChangeMemberNotificationStateTo(_validCurrentUser, memberId, State);

        member.ShouldNotify.Should().BeTrue();
        _homeMemberRepositoryMock.Verify(ns => ns.Update(It.Is<HomeMember>(hm => hm.ShouldNotify == true)),
            Times.Once);
    }

    [TestMethod]
    public void
        ChangeMemberNotificationStateTo_WhenParametersAreValidAndStateIsTrue_ShouldChangeMemberNotificationStateToFalse()
    {
        var member = new HomeMember(_validHome, _validCurrentUser);

        Guid? memberId = _validCurrentUser.Id;

        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.Id == memberId)).Returns(member);
        _homeMemberRepositoryMock.Setup(ns => ns.Update(It.IsAny<HomeMember>())).Verifiable();

        _service.ChangeMemberNotificationStateTo(_validCurrentUser, memberId, false);

        member.ShouldNotify.Should().BeFalse();
        _homeMemberRepositoryMock.Verify(ns => ns.Update(It.Is<HomeMember>(hm => hm.ShouldNotify == false)),
            Times.Once);
    }

    #endregion

    #endregion

    #region GetNotifications

    [TestMethod]
    public void GetNotifications_WhenParametersAreValid_ShouldReturnNotifications()
    {
        Home home = _validHome;
        var homeMember = new HomeMember(home, _validCurrentUser);
        home.AddMember(homeMember);
        var homeDevice = new HomeDevice(home, _validSmartDevice);
        var notification = new Notification("evt1", homeDevice, [homeMember]);
        var dto = new FilterNotificationsArgs(_validCurrentUser, null, null, null);

        List<ShowNotificationDto> expectedNotifications = [NotificationToShowNotificationDto(notification)];

        _notificationRepositoryMock.Setup(ns => ns.GetAll(n =>
            n.Members.Any(m => m.UserId == dto.CurrentUser.Id) &&
            (dto.Date == null || (n.EventDate.Day == dto.Date!.Value.Day && n.EventDate.Month == dto.Date.Value.Month &&
                                  n.EventDate.Year == dto.Date.Value.Year)) &&
            (dto.DeviceType == null || n.HomeDevice.Device.DeviceType == dto.DeviceType) &&
            (dto.IsRead == null || n.IsRead == dto.IsRead))).Returns([notification]);
        _notificationRepositoryMock.Setup(ns => ns.Update(notification)).Verifiable();

        List<ShowNotificationDto> result = _service.GetNotifications(dto);

        result.Should().BeEquivalentTo(expectedNotifications);
        notification.IsRead.Should().BeTrue();
        _notificationRepositoryMock.Verify(ns => ns.Update(notification), Times.Once);
        notification.IsRead.Should().BeTrue();
    }

    [TestMethod]
    public void GetNotifications_WhenParametersAreValidAndFilterByMotionSensor_ShouldReturnNotifications()
    {
        Home home = _validHome;
        var homeMember = new HomeMember(home, _validCurrentUser);
        home.AddMember(homeMember);
        var imageList = new List<DeviceImage> { new("logo.png", true) };
        var device2 =
            new Camera(
                new CreateCameraArgs("SmartDevices", "AAA111", "no", _validCompany, imageList, true, true, true, true));
        var homeDevice2 = new HomeDevice(home, device2);
        var notification2 = new Notification("evt2", homeDevice2, [homeMember]);

        var dto = new FilterNotificationsArgs(_validCurrentUser, "MotionSensor", null, null);

        List<ShowNotificationDto> expectedNotifications = [NotificationToShowNotificationDto(notification2)];

        _notificationRepositoryMock.Setup(ns => ns.GetAll(n =>
            n.Members.Any(m => m.UserId == dto.CurrentUser.Id) &&
            (dto.Date == null || (n.EventDate.Day == dto.Date!.Value.Day && n.EventDate.Month == dto.Date.Value.Month &&
                                  n.EventDate.Year == dto.Date.Value.Year)) &&
            (dto.DeviceType == null || n.HomeDevice.Device.DeviceType == dto.DeviceType) &&
            (dto.IsRead == null || n.IsRead == dto.IsRead))).Returns([notification2]);
        _notificationRepositoryMock.Setup(ns => ns.Update(notification2)).Verifiable();

        List<ShowNotificationDto> result = _service.GetNotifications(dto);

        result.Should().BeEquivalentTo(expectedNotifications);
        _notificationRepositoryMock.Verify(ns => ns.Update(notification2), Times.Once);
        notification2.IsRead.Should().BeTrue();
    }

    private ShowNotificationDto NotificationToShowNotificationDto(Notification notification)
    {
        var dto = new ShowNotificationDto(notification.Id, notification.Event, notification.HomeDevice.Id,
            notification.IsRead, notification.EventDate);

        return dto;
    }

    #endregion
}
