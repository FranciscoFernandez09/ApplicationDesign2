using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogic.Tests.ServicesTests;

[TestClass]
public class UserServiceTest
{
    private const string Name = "John";
    private const string LastName = "Doe";
    private const string Email = "JohnDoe@gmail.com";
    private const string Password = "Password--12";
    private const string ProfileImage = "image.jpg";
    private const int Offset = 0;
    private const int Limit = 10;

    private static readonly List<DeviceImage> DeviceImages = [new DeviceImage("url1.png", true)];

    private static readonly User User =
        new(new CreateUserArgs("John", "Doe", "johndoe@gmail.com", "Password1@!", null)
        {
            Role = new Role("HomeOwner")
        });

    private readonly Company _validCompany = new(new CreateCompanyArgs("company", User, "1234567890-1", "logo.png",
        Guid.Parse("f3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b")));

    private Mock<IRepository<SmartDevice>> _deviceRepositoryMock = null!;
    private Mock<IRepository<Role>> _roleRepositoryMock = null!;
    private Mock<IRepository<User>> _userRepositoryMock = null!;
    private UserService _userService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Strict);
        _roleRepositoryMock = new Mock<IRepository<Role>>(MockBehavior.Strict);
        _deviceRepositoryMock = new Mock<IRepository<SmartDevice>>(MockBehavior.Strict);

        _roleRepositoryMock.Setup(rs => rs.Add(It.IsAny<Role>())).Verifiable();

        _userService = new UserService(_userRepositoryMock.Object, _roleRepositoryMock.Object,
            _deviceRepositoryMock.Object);

        var homeOwner = new Role("HomeOwner");
        homeOwner.AddPermission(new SystemPermission("GetDevices"));
        homeOwner.AddPermission(new SystemPermission("GetDevicesTypes"));
    }

    #region GetDevicesTypes

    #region Success

    [TestMethod]
    public void GetDevicesTypes_WhenHasAuthorization_ShouldBeValid()
    {
        List<string> expectedTypes = _userService.GetDevicesTypes();
        expectedTypes.Should()
            .BeEquivalentTo(new List<string> { "MotionSensor", "Camera", "WindowSensor", "SmartLamp" });
    }

    #endregion

    #endregion

    #region CreateHomeOwner

    #region Error

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void CreateHomeOwner_WhenProfileImageAreNullOrEmpty_ShouldThrowArgumentNullException(string profileImage)
    {
        var args = new CreateUserArgs(Name, LastName, Email, Password, profileImage);

        _roleRepositoryMock.Setup(rs => rs.Get(r => r.Id == Constant.HomeOwnerRoleId)).Returns(new Role("HomeOwner"));

        Action act = () => _userService.CreateHomeOwner(args);

        act.Should().ThrowExactly<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'profileImage')");
    }

    [TestMethod]
    public void CreateHomeOwner_WhenEmailIsRegistered_ShouldThrowValidationException()
    {
        var args = new CreateUserArgs(Name, LastName, Email, Password, ProfileImage);

        _roleRepositoryMock.Setup(rs => rs.Get(r => r.Id == Constant.HomeOwnerRoleId)).Returns(new Role("HomeOwner"));
        _userRepositoryMock.Setup(us => us.Exists(u => u.Email == Email)).Returns(true);

        Action act = () =>
            _userService.CreateHomeOwner(args);

        act.Should().ThrowExactly<InvalidOperationException>()
            .WithMessage("Email is already registered.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateHomeOwner_WhenParametersAreValid_ShouldCreateUser()
    {
        var args = new CreateUserArgs(Name, LastName, Email, Password, ProfileImage);

        _roleRepositoryMock.Setup(rs => rs.Get(r => r.Id == Constant.HomeOwnerRoleId)).Returns(new Role("HomeOwner"));
        _userRepositoryMock.Setup(us => us.Exists(u => u.Email == Email)).Returns(false);
        _userRepositoryMock.Setup(us => us.Add(It.IsAny<User>())).Verifiable();

        _userService.CreateHomeOwner(args);

        _userRepositoryMock.Verify(us => us.Add(It.Is<User>(u =>
            u.Name == Name &&
            u.LastName == LastName &&
            u.Email == Email &&
            u.Password == Password &&
            u.ProfileImage == ProfileImage)), Times.Once);
        _userRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

    #region GetDevices

    #region Success

    [TestMethod]
    public void GetDevices_WhenTheyDontHaveCondition_ShouldReturnDevices()
    {
        var devices = new List<SmartDevice>
        {
            new(
                new CreateSmartDeviceArgs("name1", "AAA111", "description1", _validCompany,
                    "WindowSensor", DeviceImages)),
            new(
                new CreateSmartDeviceArgs("name2", "AAA111", "description2", _validCompany,
                    "MotionSensor", DeviceImages)),
            new(
                new CreateSmartDeviceArgs("name3", "AAA111", "description3", _validCompany,
                    "WindowSensor", DeviceImages))
        };
        var dto = new FilterDeviceArgs(null, null, null, null, Offset, Limit);

        _deviceRepositoryMock.Setup(sds => sds.GetAll(d =>
                (dto.DeviceType == null || d.DeviceType == dto.DeviceType) &&
                (string.IsNullOrEmpty(dto.Name) || d.Name == dto.Name) &&
                (dto.Model == null || d.Model == dto.Model) &&
                (string.IsNullOrEmpty(dto.CompanyName) || d.CompanyOwner.Name == dto.CompanyName), Offset, Limit))
            .Returns(devices);

        var expectedDevices = devices.Select(ConvertToShowDeviceDtoList).ToList();

        List<ShowDeviceDto> result = _userService.GetDevices(dto);

        result.Should().BeEquivalentTo(expectedDevices);
    }

    [TestMethod]
    public void GetDevices_WhenTheyHaveAllConditions_ShouldReturnDevices()
    {
        var devices = new List<SmartDevice>
        {
            new(
                new CreateSmartDeviceArgs("name1", "AAA111", "description1", _validCompany,
                    "WindowSensor", DeviceImages)),
            new(
                new CreateSmartDeviceArgs("name2", "AAA111", "description2", _validCompany,
                    "MotionSensor", DeviceImages)),
            new(
                new CreateSmartDeviceArgs("name3", "AAA111", "description3", _validCompany,
                    "WindowSensor", DeviceImages))
        };
        var dto = new FilterDeviceArgs("name1", "AAA111", "companyOwnerName", null, Offset, Limit);

        _deviceRepositoryMock.Setup(sds => sds.GetAll(d =>
                (dto.DeviceType == null || d.DeviceType == dto.DeviceType) &&
                (string.IsNullOrEmpty(dto.Name) || d.Name == dto.Name) &&
                (dto.Model == null || d.Model == dto.Model) &&
                (string.IsNullOrEmpty(dto.CompanyName) || d.CompanyOwner.Name == dto.CompanyName), Offset, Limit))
            .Returns(devices);

        var expectedDevices = devices.Select(ConvertToShowDeviceDtoList).ToList();

        List<ShowDeviceDto> result = _userService.GetDevices(dto);

        result.Should().BeEquivalentTo(expectedDevices);
    }

    [TestMethod]
    public void GetDevices_WhenTheyHaveSensorsAndCameras_ShouldReturnDevices()
    {
        var devices = new List<SmartDevice>
        {
            new(
                new CreateSmartDeviceArgs("name1", "AAA111", "description1", _validCompany,
                    "MotionSensor", DeviceImages)),
            new(
                new CreateSmartDeviceArgs("name2", "AAA111", "description2", _validCompany,
                    "WindowSensor", DeviceImages)),
            new(
                new CreateSmartDeviceArgs("name3", "AAA111", "description3", _validCompany,
                    "MotionSensor", DeviceImages)),
            new Camera(new CreateCameraArgs("name3", "AAA111", "description3", _validCompany,
                DeviceImages, true, true, true, true)),
            new Camera(new CreateCameraArgs("name1", "AAA111", "description1", _validCompany,
                DeviceImages, true, true, true, true)),
            new Camera(new CreateCameraArgs("name2", "AAA111", "description2", _validCompany,
                DeviceImages, true, true, true, true))
        };
        var dto = new FilterDeviceArgs("name1", "AAA111", "companyOwnerName", null, Offset, Limit);

        _deviceRepositoryMock.Setup(sds => sds.GetAll(d =>
                (dto.DeviceType == null || d.DeviceType == dto.DeviceType) &&
                (string.IsNullOrEmpty(dto.Name) || d.Name == dto.Name) &&
                (dto.Model == null || d.Model == dto.Model) &&
                (string.IsNullOrEmpty(dto.CompanyName) || d.CompanyOwner.Name == dto.CompanyName), Offset, Limit))
            .Returns(devices);

        var expectedDevices = devices.Select(MapDevicesToDto).ToList();

        List<ShowDeviceDto> result = _userService.GetDevices(dto);
        result.Should().BeEquivalentTo(expectedDevices);
    }

    [TestMethod]
    public void GetDevices_WhenTheyAreFilterByType_ShouldReturnDevices()
    {
        var devices = new List<SmartDevice>
        {
            new(
                new CreateSmartDeviceArgs("name1", "AAA111", "description1", _validCompany, "WindowSensor",
                    DeviceImages)),
            new(
                new CreateSmartDeviceArgs("name2", "AAA111", "description2", _validCompany, "MotionSensor",
                    DeviceImages)),
            new(
                new CreateSmartDeviceArgs("name3", "AAA111", "description3", _validCompany,
                    "MotionSensor", DeviceImages)),
            new Camera(new CreateCameraArgs("name1", "AAA111", "description1", _validCompany,
                DeviceImages, true, true, true, true)),
            new Camera(new CreateCameraArgs("name2", "AAA111", "description2", _validCompany,
                DeviceImages, true, true, true, true)),
            new Camera(new CreateCameraArgs("name3", "AAA111", "description3", _validCompany,
                DeviceImages, true, true, true, true))
        };

        var dto = new FilterDeviceArgs(null, null, null, "Sensor", Offset, Limit);

        _deviceRepositoryMock.Setup(ds => ds.GetAll(d =>
                (dto.DeviceType == null || d.DeviceType == dto.DeviceType) &&
                (string.IsNullOrEmpty(dto.Name) || d.Name == dto.Name) &&
                (dto.Model == null || d.Model == dto.Model) &&
                (string.IsNullOrEmpty(dto.CompanyName) || d.CompanyOwner.Name == dto.CompanyName), Offset, Limit))
            .Returns(devices);

        var expectedDevices = devices.Select(MapDevicesToDto).ToList();

        List<ShowDeviceDto> result = _userService.GetDevices(dto);

        result.Should().BeEquivalentTo(expectedDevices);
    }

    private static ShowDeviceDto ConvertToShowDeviceDtoList(SmartDevice device)
    {
        return new ShowDeviceDto(device.Id, device.Name, device.Model, device.GetMainImage().ImageUrl,
            device.CompanyOwner.Name);
    }

    private static ShowDeviceDto MapDevicesToDto(SmartDevice device)
    {
        return new ShowDeviceDto(device.Id, device.Name, device.Model, device.GetMainImage().ImageUrl,
            device.CompanyOwner.Name);
    }

    #endregion

    #endregion

    #region ModifyProfileImage

    #region Error

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void ModifyProfileImage_WhenProfileImageIsNullOrEmpty_ShouldThrowArgumentNullException(string? profileImage)
    {
        Action act = () => _userService.ModifyProfileImage(User, profileImage);

        act.Should().ThrowExactly<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'profileImage')");
    }

    [TestMethod]
    public void ModifyProfileImage_WhenProfileImageIsInvalid_ShouldThrowArgumentNullException()
    {
        const string newProfileImage = "newImage.aa";

        Action act = () => _userService.ModifyProfileImage(User, newProfileImage);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Invalid image: Must be a valid image format (jpg, png).");
    }

    #endregion

    #region Success

    [TestMethod]
    public void ModifyProfileImage_WhenProfileImageIsValid_ShouldModifyProfileImage()
    {
        const string newProfileImage = "newImage.jpg";

        _userRepositoryMock.Setup(us => us.Update(It.IsAny<User>())).Verifiable();

        _userService.ModifyProfileImage(User, newProfileImage);

        _userRepositoryMock.Verify(us => us.Update(It.Is<User>(u => u.ProfileImage == newProfileImage)), Times.Once);
        _userRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion
}
