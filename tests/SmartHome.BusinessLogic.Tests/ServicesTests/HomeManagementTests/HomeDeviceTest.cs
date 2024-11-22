using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Services.HomeManagement;

namespace SmartHome.BusinessLogic.Tests.ServicesTests.HomeManagementTests;

[TestClass]
public class HomeDeviceTest
{
    private const string DeviceName = "deviceName";
    private Mock<IRepository<HomeDevice>> _homeDeviceRepositoryMock = null!;
    private Mock<IRepository<HomeMember>> _homeMemberRepositoryMock = null!;

    private HomeDeviceService _service = null!;
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
        const string deviceType = "MotionSensor";

        _homeMemberRepositoryMock = new Mock<IRepository<HomeMember>>(MockBehavior.Strict);
        _homeDeviceRepositoryMock = new Mock<IRepository<HomeDevice>>(MockBehavior.Strict);

        _service = new HomeDeviceService(_homeDeviceRepositoryMock.Object, _homeMemberRepositoryMock.Object);

        _validCurrentUser =
            new User(new CreateUserArgs(name, lastName, email, password, profileImage)
            {
                Role = new Role { Id = Constant.HomeOwnerRoleId, Name = "HomeOwner" }
            });

        _validHome = new Home(new CreateHomeArgs(addressStreet, addressNumber, latitude, longitude,
            maxMembers, homeName, _validCurrentUser));

        var imageList = new List<DeviceImage> { new("logo.png", true) };
        var company = new Company(new CreateCompanyArgs("company", _validCurrentUser, "1234567890-1", "logo.png",
            Guid.Parse("f3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b")));
        _validSmartDevice =
            new SmartDevice(
                new CreateSmartDeviceArgs("Device", "AAA111", "Description", company, deviceType, imageList));
    }

    #region ConnectDevice

    #region Error

    [TestMethod]
    public void ConnectDevice_WhenHardwareIdIsNull_ShouldThrowArgumentNullException()
    {
        Action act = () => _service.ConnectDevice(_validCurrentUser, null);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void ConnectDevice_WhenHardwareIdIsEmpty_ShouldThrowArgumentNullException()
    {
        Action act = () => _service.ConnectDevice(_validCurrentUser, Guid.Empty);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void ConnectDevice_WhenDeviceNotAddedToHome_ShouldThrowInvalidOperationException()
    {
        Guid? hardwareId = Guid.NewGuid();

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns((HomeDevice?)null);

        Action act = () => _service.ConnectDevice(_validCurrentUser, hardwareId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device is not added to home.");
    }

    [TestMethod]
    public void ConnectDevice_WhenDeviceIsAlreadyConnected_ShouldThrowInvalidOperationException()
    {
        var homeDevice = new HomeDevice(_validHome, _validSmartDevice) { IsConnected = true };
        Guid? hardwareId = _validSmartDevice.Id;

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.ConnectDevice(_validCurrentUser, hardwareId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device is already connected.");
    }

    [TestMethod]
    public void ConnectDevice_WhenUserIsNotHomeOwner_ShouldThrowUnauthorizedAccessException()
    {
        var role = new Role("HomeOwner");
        role.AddPermission(new SystemPermission("AddDevice"));
        var user = new User
        {
            Name = "Ana",
            LastName = "Parker",
            Email = "ana@gmail.com",
            Password = "Password-123",
            Role = role
        };
        var homeDevice = new HomeDevice(_validHome, _validSmartDevice) { IsConnected = false };
        Guid? hardwareId = _validSmartDevice.Id;

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.ConnectDevice(user, hardwareId);

        act.Should().Throw<UnauthorizedAccessException>()
            .WithMessage("User does not is owner of the home.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void ConnectDevice_WhenParametersAreValid_ShouldConnectDevice()
    {
        var homeDevice = new HomeDevice(_validHome, _validSmartDevice) { IsConnected = false };
        Guid? hardwareId = _validSmartDevice.Id;

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);
        _homeDeviceRepositoryMock.Setup(hds => hds.Update(It.IsAny<HomeDevice>())).Verifiable();

        _service.ConnectDevice(_validCurrentUser, hardwareId);

        homeDevice.IsConnected.Should().BeTrue();
        _homeDeviceRepositoryMock.Verify(hds => hds.Update(It.Is<HomeDevice>(hd => hd.IsConnected == true)),
            Times.Once);
    }

    #endregion

    #endregion

    #region DisconnectDevice

    #region Error

    [TestMethod]
    public void DisconnectDevice_WhenHardwareIdIsNull_ShouldThrowArgumentNullException()
    {
        Action act = () => _service.DisconnectDevice(_validCurrentUser, null);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void DisconnectDevice_WhenHardwareIdIsEmpty_ShouldThrowArgumentNullException()
    {
        Action act = () => _service.DisconnectDevice(_validCurrentUser, Guid.Empty);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void DisconnectDevice_WhenDeviceNotAddedToHome_ShouldThrowInvalidOperationException()
    {
        Guid? hardwareId = Guid.NewGuid();

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns((HomeDevice?)null);

        Action act = () => _service.DisconnectDevice(_validCurrentUser, hardwareId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device is not added to home.");
    }

    [TestMethod]
    public void DisconnectDevice_WhenDeviceIsAlreadyDisconnected_ShouldThrowInvalidOperationException()
    {
        var homeDevice = new HomeDevice(_validHome, _validSmartDevice) { IsConnected = false };
        Guid? hardwareId = homeDevice.Id;

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.DisconnectDevice(_validCurrentUser, hardwareId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device is already disconnected.");
    }

    [TestMethod]
    public void DisconnectDevice_WhenUserIsNotHomeOwner_ShouldThrowUnauthorizedAccessException()
    {
        var role = new Role("HomeOwner");
        role.AddPermission(new SystemPermission("AddDevice"));
        var user = new User
        {
            Name = "Ana",
            LastName = "Parker",
            Email = "ana@gmail.com",
            Password = "Password-123",
            Role = role
        };
        var homeDevice = new HomeDevice(_validHome, _validSmartDevice);
        Guid? hardwareId = homeDevice.Id;

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.DisconnectDevice(user, hardwareId);

        act.Should().Throw<UnauthorizedAccessException>()
            .WithMessage("User does not is owner of the home.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void DisconnectDevice_WhenParametersAreValid_ShouldDisconnectDevice()
    {
        var homeDevice = new HomeDevice(_validHome, _validSmartDevice);
        Guid? hardwareId = homeDevice.Id;

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);
        _homeDeviceRepositoryMock.Setup(hds => hds.Update(It.IsAny<HomeDevice>())).Verifiable();

        _service.DisconnectDevice(_validCurrentUser, hardwareId);

        homeDevice.IsConnected.Should().BeFalse();
        _homeDeviceRepositoryMock.Verify(hds => hds.Update(It.Is<HomeDevice>(hd => hd.IsConnected == false)),
            Times.Once);
    }

    #endregion

    #endregion

    #region ModifyHomeDeviceName

    #region Error

    [TestMethod]
    public void ModifyHomeDeviceName_WhenHardwareIdIsNull_ShouldReturnArgumentNullException()
    {
        Action act = () => _service.ModifyHomeDeviceName(_validCurrentUser, null, DeviceName);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void ModifyHomeDeviceName_WhenHardwareIdIsEmpty_ShouldReturnArgumentNullException()
    {
        Action act = () => _service.ModifyHomeDeviceName(_validCurrentUser, Guid.Empty, DeviceName);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void ModifyHomeDeviceName_WhenHardwareNameIsNull_ShouldReturnArgumentNullException()
    {
        Action act = () => _service.ModifyHomeDeviceName(_validCurrentUser, Guid.NewGuid(), null);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'name')");
    }

    [TestMethod]
    public void ModifyHomeDeviceName_WhenHardwareNameIsEmpty_ShouldReturnArgumentNullException()
    {
        Action act = () => _service.ModifyHomeDeviceName(_validCurrentUser, Guid.NewGuid(), string.Empty);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'name')");
    }

    [TestMethod]
    public void ModifyHomeDeviceName_WhenHomeDeviceNotFound_ShouldReturnInvalidOperationException()
    {
        var homeDevice = new HomeDevice(_validHome, _validSmartDevice);
        Guid? hardwareId = homeDevice.Id;

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns((HomeDevice?)null);

        Action act = () => _service.ModifyHomeDeviceName(_validCurrentUser, hardwareId, DeviceName);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device is not added to home.");
    }

    [TestMethod]
    public void ModifyHomeDeviceName_WhenUserIsNotHomeMember_ShouldReturnUnauthorizedAccessException()
    {
        var role = new Role("HomeOwner");
        role.AddPermission(new SystemPermission("AddDevice"));
        var user = new User
        {
            Name = "Ana",
            LastName = "Parker",
            Email = "AnaParker@gmail.com",
            Password = "Password-123",
            Role = role
        };

        var homeDevice = new HomeDevice(_validHome, _validSmartDevice);
        Guid? hardwareId = homeDevice.Id;

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);
        _homeMemberRepositoryMock
            .Setup(hms => hms.Get(hm => hm.UserId == user.Id && hm.HomeId == homeDevice.HomeId))
            .Returns((HomeMember?)null);
        Action act = () => _service.ModifyHomeDeviceName(user, hardwareId, DeviceName);

        act.Should().Throw<UnauthorizedAccessException>()
            .WithMessage("User does not is member of the home.");
    }

    [TestMethod]
    public void ModifyHomeDeviceName_WhenUserIsMemberWithoutPermissions_ShouldReturnUnauthorizedAccessException()
    {
        var role = new Role("HomeOwner");
        var user = new User
        {
            Name = "Ana",
            LastName = "Parker",
            Email = "AnaParker@gmail.com",
            Password = "Password-123",
            Role = role
        };

        var homeDevice = new HomeDevice(_validHome, _validSmartDevice);
        Guid? hardwareId = homeDevice.Id;

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);
        _homeMemberRepositoryMock
            .Setup(hms => hms.Get(hm => hm.UserId == user.Id && hm.HomeId == homeDevice.HomeId))
            .Returns(new HomeMember(_validHome, user));

        Action act = () => _service.ModifyHomeDeviceName(user, hardwareId, DeviceName);

        act.Should().Throw<UnauthorizedAccessException>()
            .WithMessage("User does not have permission to modify device name.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void ModifyHomeDeviceName_WhenCurrentUserIsHomeOwner_ShouldModifyHomeDeviceName()
    {
        Guid? hardwareId = Guid.NewGuid();
        const string newDeviceName = "New Device Name";
        User user = _validCurrentUser;
        var validHomeMember = new HomeMember(_validHome, user);
        var homeDevice = new HomeDevice(_validHome, _validSmartDevice);

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);
        _homeMemberRepositoryMock
            .Setup(hms => hms.Get(hm => hm.UserId == user.Id && hm.HomeId == homeDevice.HomeId))
            .Returns(validHomeMember);
        _homeDeviceRepositoryMock.Setup(hds => hds.Update(It.IsAny<HomeDevice>())).Verifiable();

        _service.ModifyHomeDeviceName(user, hardwareId, newDeviceName);

        homeDevice.Name.Should().Be(newDeviceName);
        _homeDeviceRepositoryMock.Verify(hds => hds.Update(It.Is<HomeDevice>(hd => hd.Name == newDeviceName)),
            Times.Once);
    }

    [TestMethod]
    public void ModifyHomeDeviceName_WhenCurrentUserIsMemberWithPermissions_ShouldModifyHomeDeviceName()
    {
        var user = new User
        {
            Name = "Ana",
            LastName = "Parker",
            Email = "Ana@gmail.com",
            Password = "Password-123",
            Role = new Role("HomeOwner")
        };
        Guid? hardwareId = Guid.NewGuid();
        const string newDeviceName = "New Device Name";
        var homeDevice = new HomeDevice(_validHome, _validSmartDevice);
        var homePermissions = new List<HomePermission>
        {
            new() { Id = Constant.ModifyHomeDeviceNameId, Name = "ModifyHomeDeviceName" }
        };
        var validHomeMember = new HomeMember(_validHome, user) { Permissions = homePermissions };

        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);
        _homeMemberRepositoryMock
            .Setup(hms => hms.Get(hm => hm.UserId == user.Id && hm.HomeId == homeDevice.HomeId))
            .Returns(validHomeMember);
        _homeDeviceRepositoryMock.Setup(hds => hds.Update(It.IsAny<HomeDevice>())).Verifiable();

        _service.ModifyHomeDeviceName(user, hardwareId, newDeviceName);

        homeDevice.Name.Should().Be(newDeviceName);
        _homeDeviceRepositoryMock.Verify(hds => hds.Update(It.Is<HomeDevice>(hd => hd.Name == newDeviceName)),
            Times.Once);
    }

    #endregion

    #endregion
}
