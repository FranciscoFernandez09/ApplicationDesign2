using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.EFCoreClasses;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogic.Tests.ServicesTests;

[TestClass]
public class DeviceActionServiceTest
{
    private const bool IsActive = true;
    private Company _company = null!;
    private User _companyOwner = null!;
    private List<DeviceImage> _deviceImages = null!;
    private Home _home = null!;
    private Mock<IRepository<HomeDevice>> _homeDeviceRepositoryMock = null!;
    private Mock<IRepository<HomeMemberNotification>> _homeMemberNotificationRepositoryMock = null!;
    private Mock<IRepository<Notification>> _notificationRepositoryMock = null!;
    private DeviceActionService _service = null!;
    private User _user = null!;
    private Mock<IRepository<User>> _userRepositoryMock = null!;
    private HomeDevice _validHomeDevice = null!;

    [TestInitialize]
    public void Initialize()
    {
        _homeDeviceRepositoryMock = new Mock<IRepository<HomeDevice>>(MockBehavior.Strict);
        _notificationRepositoryMock = new Mock<IRepository<Notification>>(MockBehavior.Strict);
        _userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Strict);
        _homeMemberNotificationRepositoryMock = new Mock<IRepository<HomeMemberNotification>>(MockBehavior.Strict);
        _service = new DeviceActionService(_homeDeviceRepositoryMock.Object, _notificationRepositoryMock.Object,
            _userRepositoryMock.Object, _homeMemberNotificationRepositoryMock.Object);

        _user = new User(
            new CreateUserArgs("Ana", "Parker", "ana@gmail.com", "Password-123", null)
            {
                Role = new Role("HomeOwner")
            });
        _companyOwner = new User(
            new CreateUserArgs("Peter", "Parker", "Peter@gmail.com", "Password-123", null)
            {
                Role = new Role("CompanyOwner")
            });
        _company = new Company(new CreateCompanyArgs("Company", _companyOwner, "0123456789-0", "logo.png",
            Guid.Parse("f3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b")));
        _deviceImages = [new DeviceImage("image1.png", true), new DeviceImage("image2.png", false)];
        _home = new Home(new CreateHomeArgs("ValidAddressStreet", 123, 10, 99, 4, "name", _companyOwner));
        var camera =
            new Camera(
                new CreateCameraArgs("Camera", "AAA111", "Full", _company, _deviceImages, true, true, true, true));
        _validHomeDevice = new HomeDevice(_home, camera) { IsConnected = true };
    }

    #region PersonDetectionAction

    #region Error

    [TestMethod]
    public void PersonDetectionAction_WhenHardwareIdIsNull_ShouldTrowArgumentNullException()
    {
        Action act = () => _service.PersonDetectionAction(null, null);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void PersonDetectionAction_WhenHardwareIdIsEmpty_ShouldTrowArgumentNullException()
    {
        Action act = () => _service.PersonDetectionAction(Guid.Empty, null);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void PersonDetectionAction_WhenHardwareIdNotExist_ShouldTrowInvalidOperationException()
    {
        Guid? hardwareId = Guid.NewGuid();

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns((HomeDevice?)null);

        Action act = () => _service.PersonDetectionAction(hardwareId, null);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device is not added to home.");
    }

    [TestMethod]
    public void PersonDetectionAction_WhenSmartDeviceTypeIsNotCamera_ShouldTrowInvalidOperationException()
    {
        Guid? hardwareId = Guid.NewGuid();

        var smartDevice =
            new SmartDevice(new CreateSmartDeviceArgs("SmartDevice", "AAA111", "Full", _company, "MotionSensor",
                _deviceImages));
        var homeDevice = new HomeDevice(_home, smartDevice);

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.PersonDetectionAction(hardwareId, null);

        act.Should().Throw<InvalidOperationException>().WithMessage("Smart device is not a camera.");
    }

    [TestMethod]
    public void PersonDetectionAction_WhenCameraIsNotConnected_ShouldTrowInvalidOperationException()
    {
        Guid? hardwareId = Guid.NewGuid();

        _validHomeDevice.IsConnected = false;

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(_validHomeDevice);

        Action act = () => _service.PersonDetectionAction(hardwareId, null);

        act.Should().Throw<InvalidOperationException>().WithMessage("Camera is not connected.");
    }

    [TestMethod]
    public void PersonDetectionAction_WhenCameraHasNotDetectionPerson_ShouldTrowInvalidOperationException()
    {
        Guid? hardwareId = Guid.NewGuid();

        var camera =
            new Camera(new CreateCameraArgs("Camera", "AAA111", "Full", _company, _deviceImages, true, true, true,
                false));
        var homeDevice = new HomeDevice(_home, camera) { IsConnected = true };

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.PersonDetectionAction(hardwareId, null);

        act.Should().Throw<InvalidOperationException>().WithMessage("Camera has not detection person.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void PersonDetectionAction_WhenPersonIdIsNull_ShouldCreateNotification()
    {
        Guid? hardwareId = Guid.NewGuid();

        var member1 = new HomeMember(_home, _companyOwner) { ShouldNotify = true };
        var member2 = new HomeMember(_home, _user);
        Home home = _home;
        home.AddMember(member1);
        home.AddMember(member2);

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(_validHomeDevice);
        _notificationRepositoryMock.Setup(ns => ns.Add(It.IsAny<Notification>())).Verifiable();
        _homeMemberNotificationRepositoryMock.Setup(hms => hms.Add(It.IsAny<HomeMemberNotification>())).Verifiable();

        _service.PersonDetectionAction(hardwareId, null);

        _notificationRepositoryMock.Verify(ns => ns.Add(It.Is<Notification>(n =>
            n.Id != Guid.Empty &&
            n.HomeDevice == _validHomeDevice &&
            n.Members.SequenceEqual(new List<HomeMember>()) &&
            n.IsRead == false &&
            n.EventDate <= DateTime.Now &&
            n.Event == "Unknown person detected")), Times.Once);
        _notificationRepositoryMock.VerifyAll();
        _homeMemberNotificationRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void PersonDetectionAction_WhenUnknownPersonId_ShouldCreateNotification()
    {
        Guid? hardwareId = Guid.NewGuid();

        var user = new User(
            new CreateUserArgs("Peter", "Parker", "Peter2@gmail.com", "Password-123", null)
            {
                Role = new Role("CompanyOwner")
            });
        var member1 = new HomeMember(_home, _companyOwner) { ShouldNotify = true };
        var member2 = new HomeMember(_home, user);
        Home home = _home;
        home.AddMember(member1);
        home.AddMember(member2);

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(_validHomeDevice);
        _notificationRepositoryMock.Setup(ns => ns.Add(It.IsAny<Notification>())).Verifiable();
        _userRepositoryMock.Setup(us => us.Exists(u => u.Id == user.Id)).Returns(false);
        _homeMemberNotificationRepositoryMock.Setup(hms => hms.Add(It.IsAny<HomeMemberNotification>())).Verifiable();

        _service.PersonDetectionAction(hardwareId, user);

        _notificationRepositoryMock.Verify(ns => ns.Add(It.Is<Notification>(n =>
            n.Id != Guid.Empty &&
            n.HomeDevice == _validHomeDevice &&
            n.Members.SequenceEqual(new List<HomeMember>()) &&
            n.IsRead == false &&
            n.EventDate <= DateTime.Now &&
            n.Event == "Unknown person detected")), Times.Once);
        _notificationRepositoryMock.VerifyAll();
        _homeMemberNotificationRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void PersonDetectionAction_WhenValidPersonId_ShouldCreateNotification()
    {
        Guid? hardwareId = Guid.NewGuid();

        var user = new User(
            new CreateUserArgs("Peter", "Parker", "Peter2@gmail.com", "Password-123", null)
            {
                Role = new Role("CompanyOwner")
            });
        var member1 = new HomeMember(_home, _companyOwner) { ShouldNotify = true };
        var member2 = new HomeMember(_home, user);
        Home home = _home;
        home.AddMember(member1);
        home.AddMember(member2);

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(_validHomeDevice);
        _notificationRepositoryMock.Setup(ns => ns.Add(It.IsAny<Notification>())).Verifiable();
        _userRepositoryMock.Setup(us => us.Exists(u => u.Id == user.Id)).Returns(true);
        _homeMemberNotificationRepositoryMock.Setup(hms => hms.Add(It.IsAny<HomeMemberNotification>())).Verifiable();

        _service.PersonDetectionAction(hardwareId, user);

        _notificationRepositoryMock.Verify(ns => ns.Add(It.Is<Notification>(n =>
            n.Id != Guid.Empty &&
            n.HomeDevice == _validHomeDevice &&
            n.Members.SequenceEqual(new List<HomeMember>()) &&
            n.IsRead == false &&
            n.EventDate <= DateTime.Now &&
            n.Event == $"Person {_companyOwner.Name} {_companyOwner.LastName} detected")), Times.Once);
        _notificationRepositoryMock.VerifyAll();
        _homeMemberNotificationRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

    #region CameraMovementDetectionAction

    #region Error

    [TestMethod]
    public void MovementDetectionAction_WhenHardwareIdIsNull_ShouldTrowArgumentNullException()
    {
        Action act = () => _service.CameraMovementDetectionAction(null);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void MovementDetectionAction_WhenHardwareIdIsEmpty_ShouldTrowArgumentNullException()
    {
        Action act = () => _service.CameraMovementDetectionAction(Guid.Empty);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void MovementDetectionAction_WhenHardwareIdNotExists_ShouldTrowInvalidOperationException()
    {
        Guid? hardwareId = Guid.NewGuid();

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns((HomeDevice?)null);

        Action act = () => _service.CameraMovementDetectionAction(hardwareId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device is not added to home.");
    }

    [TestMethod]
    public void MovementDetectionAction_WhenSmartDeviceTypeIsNotCamera_ShouldTrowInvalidOperationException()
    {
        Guid? hardwareId = Guid.NewGuid();
        var smartDevice =
            new SmartDevice(new CreateSmartDeviceArgs("SmartDevice", "AAA111", "Full", _company, "MotionSensor",
                _deviceImages));
        var homeDevice = new HomeDevice(_home, smartDevice);

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.CameraMovementDetectionAction(hardwareId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Smart device is not a camera.");
    }

    [TestMethod]
    public void MovementDetectionAction_WhenCameraIsNotConnected_ShouldTrowInvalidOperationException()
    {
        Guid? hardwareId = Guid.NewGuid();
        _validHomeDevice.IsConnected = false;

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(_validHomeDevice);

        Action act = () => _service.CameraMovementDetectionAction(hardwareId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Camera is not connected.");
    }

    [TestMethod]
    public void MovementDetectionAction_WhenCameraHasNotMotionDetection_ShouldTrowInvalidOperationException()
    {
        Guid? hardwareId = Guid.NewGuid();
        var camera =
            new Camera(new CreateCameraArgs("Camera", "AAA111", "Full", _company, _deviceImages, true, true, false,
                true));
        var homeDevice = new HomeDevice(_home, camera) { IsConnected = true };

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.CameraMovementDetectionAction(hardwareId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Camera has not motion detection.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void MovementDetectionAction_WhenParameterAreValid_ShouldCreateNotification()
    {
        Guid? hardwareId = Guid.NewGuid();
        var member1 = new HomeMember(_home, _companyOwner) { ShouldNotify = true };
        var member2 = new HomeMember(_home, _user);
        Home home = _home;
        home.AddMember(member1);
        home.AddMember(member2);

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(_validHomeDevice);
        _notificationRepositoryMock.Setup(ns => ns.Add(It.IsAny<Notification>())).Verifiable();
        _homeMemberNotificationRepositoryMock.Setup(hms => hms.Add(It.IsAny<HomeMemberNotification>())).Verifiable();

        _service.CameraMovementDetectionAction(hardwareId);

        _notificationRepositoryMock.Verify(ns => ns.Add(It.Is<Notification>(n =>
            n.Id != Guid.Empty &&
            n.HomeDevice == _validHomeDevice &&
            n.Members.SequenceEqual(new List<HomeMember>()) &&
            n.IsRead == false &&
            n.EventDate <= DateTime.Now &&
            n.Event == "Movement detected")), Times.Once);
        _notificationRepositoryMock.VerifyAll();
        _homeMemberNotificationRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

    #region ChangeWindowSensorStateTo

    #region Error

    [TestMethod]
    public void ChangeWindowSensorStateTo_WhenHardwareIdIsNull_ShouldTrowArgumentNullException()
    {
        Action act = () => _service.ChangeWindowSensorStateTo(null, IsActive);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void ChangeWindowSensorStateTo_WhenHardwareIdIsEmpty_ShouldTrowArgumentNullException()
    {
        Action act = () => _service.ChangeWindowSensorStateTo(Guid.Empty, IsActive);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void ChangeWindowSensorStateTo_WhenHardwareIdNotExists_ShouldTrowInvalidOperationException()
    {
        Guid? hardwareId = Guid.NewGuid();

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns((HomeDevice?)null);

        Action act = () => _service.ChangeWindowSensorStateTo(hardwareId, IsActive);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device is not added to home.");
    }

    [TestMethod]
    public void ChangeWindowSensorStateTo_WhenWindowSensorTypeIsNotSensor_ShouldTrowInvalidOperationException()
    {
        Guid? hardwareId = _home.Id;

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(_validHomeDevice);

        Action act = () => _service.ChangeWindowSensorStateTo(hardwareId, IsActive);

        act.Should().Throw<InvalidOperationException>().WithMessage("Smart device is not a window sensor.");
    }

    [TestMethod]
    public void ChangeWindowSensorStateTo_WhenWindowSensorIsNotConnected_ShouldTrowInvalidOperationException()
    {
        Guid? hardwareId = _home.Id;
        var sensorArgs = new CreateSmartDeviceArgs("Sensor", "AAA111", "Full", _company, "WindowSensor", _deviceImages);
        var homeDevice = new HomeDevice(_home, new SmartDevice(sensorArgs)) { IsConnected = false };

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.ChangeWindowSensorStateTo(hardwareId, IsActive);

        act.Should().Throw<InvalidOperationException>().WithMessage("Window sensor is not connected.");
    }

    [TestMethod]
    public void ChangeWindowSensorStateTo_WhenNewStatIsOpenAndAlreadyOpened_ShouldThrowInvalidOperationException()
    {
        Guid? hardwareId = _home.Id;
        var sensorArgs = new CreateSmartDeviceArgs("Sensor", "AAA111", "Full", _company, "WindowSensor", _deviceImages);
        var homeDevice = new HomeDevice(_home, new SmartDevice(sensorArgs)) { IsConnected = true, IsActive = true };

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.ChangeWindowSensorStateTo(hardwareId, homeDevice.IsActive);

        act.Should().Throw<InvalidOperationException>().WithMessage("Window sensor is already opened.");
    }

    [TestMethod]
    public void ChangeWindowSensorStateTo_WhenNewStatIsCloseAndAlreadyClosed_ShouldThrowInvalidOperationException()
    {
        Guid? hardwareId = _home.Id;
        var sensorArgs = new CreateSmartDeviceArgs("Sensor", "AAA111", "Full", _company, "WindowSensor", _deviceImages);
        var homeDevice = new HomeDevice(_home, new SmartDevice(sensorArgs)) { IsConnected = true, IsActive = false };

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.ChangeWindowSensorStateTo(hardwareId, homeDevice.IsActive);

        act.Should().Throw<InvalidOperationException>().WithMessage("Window sensor is already closed.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void ChangeWindowSensorStateTo_WhenParameterAreValidAndNewStateIsOpen_ShouldNotThrowException()
    {
        Guid? hardwareId = Guid.NewGuid();
        var member1 = new HomeMember(_home, _companyOwner) { ShouldNotify = true };
        var member2 = new HomeMember(_home, _user);
        Home home = _home;
        var sensorArgs = new CreateSmartDeviceArgs("Sensor", "AAA111", "Full", _company, "WindowSensor", _deviceImages);
        var homeDevice = new HomeDevice(_home, new SmartDevice(sensorArgs)) { IsConnected = true, IsActive = false };
        home.AddMember(member1);
        home.AddMember(member2);

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);
        _notificationRepositoryMock.Setup(ns => ns.Add(It.IsAny<Notification>())).Verifiable();
        _homeMemberNotificationRepositoryMock.Setup(hms => hms.Add(It.IsAny<HomeMemberNotification>())).Verifiable();
        _homeDeviceRepositoryMock.Setup(hds => hds.Update(homeDevice)).Verifiable();

        _service.ChangeWindowSensorStateTo(hardwareId, IsActive);

        _notificationRepositoryMock.Verify(ns => ns.Add(It.Is<Notification>(n =>
            n.Id != Guid.Empty &&
            n.HomeDevice == homeDevice &&
            n.Members.SequenceEqual(new List<HomeMember>()) &&
            n.IsRead == false &&
            n.EventDate <= DateTime.Now &&
            n.Event == "Window state opened")), Times.Once);
        _notificationRepositoryMock.VerifyAll();
        _homeMemberNotificationRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void ChangeWindowSensorStateTo_WhenParameterAreValidAndNewStateIsClose_ShouldNotThrowException()
    {
        Guid? hardwareId = Guid.NewGuid();
        var member1 = new HomeMember(_home, _companyOwner) { ShouldNotify = true };
        var member2 = new HomeMember(_home, _user);
        Home home = _home;
        var sensorArgs = new CreateSmartDeviceArgs("Sensor", "AAA111", "Full", _company, "WindowSensor", _deviceImages);
        var homeDevice = new HomeDevice(_home, new SmartDevice(sensorArgs)) { IsConnected = true, IsActive = true };
        home.AddMember(member1);
        home.AddMember(member2);

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);
        _notificationRepositoryMock.Setup(ns => ns.Add(It.IsAny<Notification>())).Verifiable();
        _homeMemberNotificationRepositoryMock.Setup(hms => hms.Add(It.IsAny<HomeMemberNotification>())).Verifiable();
        _homeDeviceRepositoryMock.Setup(hds => hds.Update(homeDevice)).Verifiable();

        _service.ChangeWindowSensorStateTo(hardwareId, !IsActive);

        _notificationRepositoryMock.Verify(ns => ns.Add(It.Is<Notification>(n =>
            n.Id != Guid.Empty &&
            n.HomeDevice == homeDevice &&
            n.Members.SequenceEqual(new List<HomeMember>()) &&
            n.IsRead == false &&
            n.EventDate <= DateTime.Now &&
            n.Event == "Window state closed")), Times.Once);
        _notificationRepositoryMock.VerifyAll();
        _homeMemberNotificationRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

    #region ChangeSmartLampStateTo

    #region Error

    [TestMethod]
    public void ChangeSmartLampStateTo_WhenHardwareIdIsNull_ShouldThrowArgumentNullException()
    {
        Action act = () => _service.ChangeSmartLampStateTo(_user, null, IsActive);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void ChangeSmartLampStateTo_WhenHardwareIdIsEmpty_ShouldThrowArgumentNullException()
    {
        Action act = () => _service.ChangeSmartLampStateTo(_user, null, IsActive);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void ChangeSmartLampStateTo_WhenHardwareIdNotExists_ShouldThrowInvalidOperationException()
    {
        Guid? hardwareId = Guid.NewGuid();

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns((HomeDevice?)null);

        Action act = () => _service.ChangeSmartLampStateTo(_user, hardwareId, IsActive);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device is not added to home.");
    }

    [TestMethod]
    public void ChangeSmartLampStateTo_WhenWindowSmartLampTypeIsNotSmartLamp_ShouldTrowInvalidOperationException()
    {
        Guid? hardwareId = _home.Id;

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(_validHomeDevice);

        Action act = () => _service.ChangeSmartLampStateTo(_user, hardwareId, IsActive);

        act.Should().Throw<InvalidOperationException>().WithMessage("Smart device is not a smart lamp.");
    }

    [TestMethod]
    public void ChangeSmartLampStateTo_WhenWindowSmartLampIsNotConnected_ShouldTrowInvalidOperationException()
    {
        Guid? hardwareId = _home.Id;
        var sensorArgs =
            new CreateSmartDeviceArgs("Smart Lamp", "AAA111", "Full", _company, "SmartLamp", _deviceImages);
        var homeDevice = new HomeDevice(_home, new SmartDevice(sensorArgs)) { IsConnected = false };

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.ChangeSmartLampStateTo(_user, hardwareId, IsActive);

        act.Should().Throw<InvalidOperationException>().WithMessage("Smart lamp is not connected.");
    }

    [TestMethod]
    public void ChangeSmartLampStateTo_WhenNewStateIsOnAndAlreadyOn_ShouldThrowInvalidOperationException()
    {
        Guid? hardwareId = Guid.NewGuid();
        var member = new HomeMember(_home, _user) { ShouldNotify = true };
        Home home = _home;
        home.AddMember(member);

        var args = new CreateSmartDeviceArgs("Smart Lamp", "AAA111", "Full", _company, "SmartLamp", _deviceImages);
        var smartLamp = new SmartDevice(args);
        var homeDevice = new HomeDevice(_home, smartLamp) { IsConnected = true, IsActive = true };

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.ChangeSmartLampStateTo(_user, hardwareId, IsActive);

        act.Should().Throw<InvalidOperationException>().WithMessage("Smart lamp is already turned on.");
    }

    [TestMethod]
    public void ChangeSmartLampStateTo_WhenNewStateIsOffAndAlreadyOff_ShouldThrowInvalidOperationException()
    {
        Guid? hardwareId = Guid.NewGuid();
        var member = new HomeMember(_home, _user) { ShouldNotify = true };
        Home home = _home;
        home.AddMember(member);

        var args = new CreateSmartDeviceArgs("Smart Lamp", "AAA111", "Full", _company, "SmartLamp", _deviceImages);
        var smartLamp = new SmartDevice(args);
        var homeDevice = new HomeDevice(_home, smartLamp) { IsConnected = true, IsActive = false };

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.ChangeSmartLampStateTo(_user, hardwareId, !IsActive);

        act.Should().Throw<InvalidOperationException>().WithMessage("Smart lamp is already turned off.");
    }

    [TestMethod]
    public void ChangeSmartLampStateTo_WhenUserIsNotHomeMember_ShouldThrowInvalidOperationException()
    {
        Guid? hardwareId = _home.Id;

        var args = new CreateSmartDeviceArgs("Smart Lamp", "AAA111", "Full", _company, "SmartLamp", _deviceImages);
        var homeDevice = new HomeDevice(_home, new SmartDevice(args)) { IsConnected = true };

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.ChangeSmartLampStateTo(_user, hardwareId, IsActive);

        act.Should().Throw<InvalidOperationException>().WithMessage("User is not a member of the home.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void ChangeSmartLampStateTo_WhenParametersAreValidAndNewStateIsOff_ShouldNotThrowException()
    {
        Guid? hardwareId = Guid.NewGuid();
        var member = new HomeMember(_home, _user) { ShouldNotify = true };
        Home home = _home;
        home.AddMember(member);

        var args = new CreateSmartDeviceArgs("Smart Lamp", "AAA111", "Full", _company, "SmartLamp", _deviceImages);
        var smartLamp = new SmartDevice(args);
        var homeDevice = new HomeDevice(_home, smartLamp) { IsConnected = true, IsActive = true };

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);
        _notificationRepositoryMock.Setup(ns => ns.Add(It.IsAny<Notification>())).Verifiable();
        _homeMemberNotificationRepositoryMock.Setup(hms => hms.Add(It.IsAny<HomeMemberNotification>()))
            .Verifiable();
        _homeDeviceRepositoryMock.Setup(hds => hds.Update(homeDevice)).Verifiable();

        _service.ChangeSmartLampStateTo(_user, hardwareId, !IsActive);

        _notificationRepositoryMock.Verify(ns => ns.Add(It.Is<Notification>(n =>
            n.Id != Guid.Empty &&
            n.HomeDevice == homeDevice &&
            n.Members.SequenceEqual(new List<HomeMember>()) &&
            n.IsRead == false &&
            n.EventDate <= DateTime.Now &&
            n.Event == "Smart lamp turned off")), Times.Once);
        _notificationRepositoryMock.VerifyAll();
        _homeMemberNotificationRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void ChangeSmartLampStateTo_WhenParametersAreValidAndNewStateIsOn_ShouldNotThrowException()
    {
        Guid? hardwareId = Guid.NewGuid();
        var member = new HomeMember(_home, _user) { ShouldNotify = true };
        Home home = _home;
        home.AddMember(member);

        var args = new CreateSmartDeviceArgs("Smart Lamp", "AAA111", "Full", _company, "SmartLamp", _deviceImages);
        var smartLamp = new SmartDevice(args);
        var homeDevice = new HomeDevice(_home, smartLamp) { IsConnected = true, IsActive = false };

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);
        _notificationRepositoryMock.Setup(ns => ns.Add(It.IsAny<Notification>())).Verifiable();
        _homeMemberNotificationRepositoryMock.Setup(hms => hms.Add(It.IsAny<HomeMemberNotification>()))
            .Verifiable();
        _homeDeviceRepositoryMock.Setup(hds => hds.Update(homeDevice)).Verifiable();

        _service.ChangeSmartLampStateTo(_user, hardwareId, IsActive);

        _notificationRepositoryMock.Verify(ns => ns.Add(It.Is<Notification>(n =>
            n.Id != Guid.Empty &&
            n.HomeDevice == homeDevice &&
            n.Members.SequenceEqual(new List<HomeMember>()) &&
            n.IsRead == false &&
            n.EventDate <= DateTime.Now &&
            n.Event == "Smart lamp turned on")), Times.Once);
        _notificationRepositoryMock.VerifyAll();
        _homeMemberNotificationRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

    #region MotionSensorMovementDetection

    #region Error

    [TestMethod]
    public void MotionSensorMovementDetection_WhenHardwareIdIsEmpty_ShouldThrowArgumentNullException()
    {
        Action act = () => _service.MotionSensorMovementDetection(Guid.Empty);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void MotionSensorMovementDetection_WhenHardwareIdIsNull_ShouldThrowArgumentNullException()
    {
        Action act = () => _service.MotionSensorMovementDetection(null);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void MotionSensorMovementDetection_WhenHardwareIdNotExists_ShouldThrowInvalidOperationException()
    {
        Guid? hardwareId = Guid.NewGuid();

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns((HomeDevice?)null);

        Action act = () => _service.MotionSensorMovementDetection(hardwareId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device is not added to home.");
    }

    [TestMethod]
    public void
        MotionSensorMovementDetection_WhenSmartDeviceTypeIsNotMotionSensor_ShouldThrowInvalidOperationException()
    {
        Guid? hardwareId = Guid.NewGuid();
        var smartDevice =
            new SmartDevice(new CreateSmartDeviceArgs("SmartDevice", "AAA111", "Full", _company, "WindowSensor",
                _deviceImages));
        var homeDevice = new HomeDevice(_home, smartDevice);

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.MotionSensorMovementDetection(hardwareId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Smart device is not a motion sensor.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void MotionSensorMovementDetection_WhenParametersAreValid_ShouldNotify()
    {
        Guid? hardwareId = Guid.NewGuid();
        var member1 = new HomeMember(_home, _companyOwner) { ShouldNotify = true };
        var member2 = new HomeMember(_home, _user);
        Home home = _home;
        home.AddMember(member1);
        home.AddMember(member2);

        var args = new CreateSmartDeviceArgs("Motion Sensor", "AAA111", "Full", _company, "MotionSensor",
            _deviceImages);
        var homeDevice = new HomeDevice(_home, new SmartDevice(args)) { IsConnected = true };

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);
        _notificationRepositoryMock.Setup(ns => ns.Add(It.IsAny<Notification>())).Verifiable();
        _homeMemberNotificationRepositoryMock.Setup(hms => hms.Add(It.IsAny<HomeMemberNotification>()))
            .Verifiable();

        _service.MotionSensorMovementDetection(hardwareId);

        _notificationRepositoryMock.Verify(ns => ns.Add(It.Is<Notification>(n =>
            n.Id != Guid.Empty &&
            n.HomeDevice == homeDevice &&
            n.Members.SequenceEqual(new List<HomeMember>()) &&
            n.HomeDevice.IsMotionSensor() &&
            n.IsRead == false &&
            n.EventDate <= DateTime.Now &&
            n.Event == "Movement detected")), Times.Once);
    }

    #endregion

    #endregion
}
