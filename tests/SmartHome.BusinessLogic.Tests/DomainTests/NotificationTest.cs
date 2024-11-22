using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

namespace SmartHome.BusinessLogic.Tests.DomainTests;

[TestClass]
public class NotificationTest
{
    private const string Event = "Event";

    private HomeDevice _validHomeDevice = null!;
    private List<HomeMember> _validHomeMembers = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        const string validUserName = "John";
        const string validUserLastName = "Doe";
        const string validUserEmail = "JohnDoe@gmail.com";
        const string validUserPassword = "Password123!";
        var validRole = new Role("Admin");
        const string validProfilePicture = "url.png";
        const string validDeviceType = "MotionSensor";

        var validUser =
            new User(new CreateUserArgs(validUserName, validUserLastName, validUserEmail, validUserPassword,
                validProfilePicture)
            { Role = validRole });

        const string validAddressStreet = "street";
        const int validAddressNumber = 1;
        const int validLatitude = 1;
        const int validLongitude = 1;
        const int validMaxMembers = 1;
        const string validHomeName = "name";

        var validHome = new Home(new CreateHomeArgs(validAddressStreet, validAddressNumber, validLatitude,
            validLongitude, validMaxMembers, validHomeName, validUser));

        const string validCompanyName = "Company";
        const string validRut = "0123456789-0";
        const string validCompanyLogo = "url.png";

        var validatorId = Guid.Parse("f3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b");
        var validCompany =
            new Company(new CreateCompanyArgs(validCompanyName, validUser, validRut, validCompanyLogo, validatorId));

        const string validImageUrl = "url.jpg";
        var validDeviceImage = new DeviceImage(validImageUrl, true);
        var validDeviceImages = new List<DeviceImage> { validDeviceImage };
        const string validSmartDeviceName = "Device";
        const string validSmartDeviceSerialModel = "AAA111";
        const string validSmartDeviceDescription = "Description";

        var validSmartDevice = new SmartDevice(new CreateSmartDeviceArgs(validSmartDeviceName,
            validSmartDeviceSerialModel, validSmartDeviceDescription, validCompany, validDeviceType,
            validDeviceImages));

        _validHomeDevice = new HomeDevice(validHome, validSmartDevice);
        _validHomeMembers = [new HomeMember(validHome, validUser)];
    }

    #region CreateNotification

    #region Error

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void Notification_WhenParametersAreNullOrEmpty_ShouldThrowArgumentNullException(string evt)
    {
        Func<Notification> act = () => new Notification(evt, _validHomeDevice, _validHomeMembers);

        act.Should().ThrowExactly<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'evt')");
    }

    [TestMethod]
    public void Notification_WhenHomeDeviceIsNull_ShouldThrowArgumentNullException()
    {
        Func<Notification> act = () => new Notification(Event, null!, _validHomeMembers);

        act.Should().ThrowExactly<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'homeDevice')");
    }

    [TestMethod]
    public void Notification_WhenHomeMemberIsNull_ShouldThrowArgumentNullException()
    {
        Func<Notification> act = () => new Notification(Event, _validHomeDevice, null!);

        act.Should().ThrowExactly<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'members')");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Notification_WhenValidParameters_ShouldCreateNotification()
    {
        var notification = new Notification(Event, _validHomeDevice, _validHomeMembers);

        notification.Id.Should().NotBeEmpty();
        notification.Event.Should().Be(Event);
        notification.IsRead.Should().Be(false);
        (notification.EventDate <= DateTime.Now).Should().BeTrue();
        notification.HomeDevice.Should().Be(_validHomeDevice);
        notification.HomeDeviceId.Should().Be(_validHomeDevice.Id);
        notification.Members.Should().BeEquivalentTo(_validHomeMembers);
    }

    #endregion

    #endregion
}
