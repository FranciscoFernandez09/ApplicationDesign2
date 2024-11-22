using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogic.Tests.ServicesTests;

[TestClass]
public class CompanyOwnerServiceTest
{
    private const string CompanyName = "Company";
    private const string CompanyRut = "0123456789-0";
    private const string CompanyLogo = "logo.png";

    private const string DeviceName = "Device";
    private const string Model = "AAA111";
    private const string DeviceDescription = "Description";

    private const bool CameraExternalUse = true;
    private const bool CameraInternalUse = true;
    private const bool CameraMotionDetection = true;
    private const bool CameraPersonDetection = true;

    private static Company _validCompany = null!;
    private readonly Guid _validatorId = Guid.Parse("f3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b");

    private Mock<IRepository<Company>> _companyRepositoryMock = null!;
    private Mock<IDeviceImporterService> _importerServiceMock = null!;
    private Mock<IRepository<Role>> _roleRepositoryMock = null!;
    private CompanyOwnerService _service = null!;
    private Mock<IRepository<SmartDevice>> _smartDeviceRepositoryMock = null!;
    private Mock<IRepository<User>> _userRepositoryMock = null!;
    private Mock<IModelValidatorService> _validatorServiceMock = null!;
    private User _validCurrentUser = null!;
    private List<DeviceImage> _validImages = null!;

    [TestInitialize]
    public void Initialize()
    {
        const string validName = "John";
        const string validLastName = "Doe";
        const string validPassword = "Password--12";
        const string validProfileImage = "";

        _userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Strict);
        _companyRepositoryMock = new Mock<IRepository<Company>>(MockBehavior.Strict);
        _smartDeviceRepositoryMock = new Mock<IRepository<SmartDevice>>(MockBehavior.Strict);
        _roleRepositoryMock = new Mock<IRepository<Role>>(MockBehavior.Strict);
        _validatorServiceMock = new Mock<IModelValidatorService>(MockBehavior.Strict);
        _importerServiceMock = new Mock<IDeviceImporterService>(MockBehavior.Strict);

        _service = new CompanyOwnerService(_userRepositoryMock.Object, _companyRepositoryMock.Object,
            _smartDeviceRepositoryMock.Object, _roleRepositoryMock.Object, _validatorServiceMock.Object,
            _importerServiceMock.Object);

        const string currentUserEmail = "Alan@gmail.com";
        var superAdmin = new Role("CompanyOwner");
        superAdmin.Permissions.Add(new SystemPermission { Id = Constant.CreateCompanyId, Name = "CreateCompany" });
        superAdmin.Permissions.Add(new SystemPermission { Id = Constant.CreateDeviceId, Name = "CreateDevice" });

        _validCurrentUser =
            new User(new CreateUserArgs(validName, validLastName, currentUserEmail, validPassword, validProfileImage)
            {
                Role = new Role { Id = Constant.CompanyOwnerRoleId, Name = "CreateCompany" }
            });

        _validCompany =
            new Company(new CreateCompanyArgs(CompanyName, _validCurrentUser, CompanyRut, CompanyLogo, _validatorId));

        var deviceImageDto = new DeviceImage("url.png", true);

        _validImages = [deviceImageDto];
    }

    #region ChangeRoleToCompanyAndHomeOwner

    [TestMethod]
    public void ChangeRoleToCompanyAndHomeOwner_WhenIsValid_ShouldChangeRoleToCompanyAndHomeOwner()
    {
        _validCurrentUser.Role = new Role { Id = Constant.AdminRoleId, Name = "CompanyOwner" };
        var expectedRole = new Role { Id = Constant.AdminHomeOwnerRoleId, Name = "CompanyAndHomeOwner" };

        _roleRepositoryMock.Setup(rs => rs.Get(r => r.Id == Constant.CompanyAndHomeOwnerRoleId)).Returns(expectedRole);
        _userRepositoryMock.Setup(us => us.Update(_validCurrentUser)).Verifiable();

        _service.ChangeRoleToCompanyAndHomeOwner(_validCurrentUser);

        _validCurrentUser.Role.Should().Be(expectedRole);
        _userRepositoryMock.Verify(us => us.Update(_validCurrentUser), Times.Once);
        _userRepositoryMock.VerifyAll();
    }

    #endregion

    #region GetModelValidators

    [TestMethod]
    public void GetModelValidators_WhenHasValidParameters_ShouldReturnModelValidators()
    {
        var expectedValidators =
            new List<ShowModelValidatorsDto> { new(Guid.NewGuid(), "Validator1"), new(Guid.NewGuid(), "Validator2") };

        _validatorServiceMock.Setup(vs => vs.GetImplementations()).Returns(expectedValidators);

        List<ShowModelValidatorsDto> result = _service.GetModelValidators();

        result.Should().BeEquivalentTo(expectedValidators);
    }

    #endregion

    #region ImportDevices

    [TestMethod]
    public void ImportDevices_WhenValidArgs_ShouldImportDevices()
    {
        var ddlId = Guid.NewGuid();
        var parameters = new Dictionary<string, string>();
        var devices = new List<SmartDevice> { new() };
        Guid userId = _validCurrentUser.Id;

        _importerServiceMock.Setup(ims => ims.ImportDevices(parameters, ddlId, _validCompany)).Returns(devices);
        _companyRepositoryMock.Setup(cs => cs.Get(c => c.OwnerId == userId)).Returns(_validCompany);
        _smartDeviceRepositoryMock.Setup(sds => sds.Add(It.IsAny<SmartDevice>())).Verifiable();

        _service.ImportDevices(ddlId, parameters, _validCurrentUser);

        _smartDeviceRepositoryMock.Verify(sds => sds.Add(It.IsAny<SmartDevice>()), Times.Once);
        _smartDeviceRepositoryMock.VerifyAll();
    }

    #endregion

    #region GetDeviceImporters

    [TestMethod]
    public void GetDeviceImporters_WhenHasValidParameters_ShouldReturnDeviceImporters()
    {
        var expectedImporters =
            new List<ShowImporterDto> { new(Guid.NewGuid(), "Importer1"), new(Guid.NewGuid(), "Importer2") };

        _importerServiceMock.Setup(ims => ims.GetImporters()).Returns(expectedImporters);

        List<ShowImporterDto> result = _service.GetDeviceImporters();

        result.Should().BeEquivalentTo(expectedImporters);
    }

    #endregion

    #region CreateCompany

    #region Error

    [TestMethod]
    public void CreteCompany_WhenUserAlreadyHasACompany_ShouldThrowArgumentException()
    {
        _validCurrentUser.HasCompany = true;
        var args = new CreateCompanyArgs(CompanyName, _validCurrentUser, CompanyRut, CompanyLogo, _validatorId);

        Action act = () => _service.CreateCompany(args);

        act.Should().Throw<InvalidOperationException>().WithMessage("User already has a company.");
    }

    [TestMethod]
    public void CreateCompany_WhenValidatorIdIsInvalid_ShouldThrowArgumentException()
    {
        var validatorId = Guid.NewGuid();
        var args = new CreateCompanyArgs(CompanyName, _validCurrentUser, CompanyRut, CompanyLogo, validatorId);

        _validatorServiceMock.Setup(vs => vs.ValidatorIdIsValid(validatorId)).Returns(false);

        Action act = () => _service.CreateCompany(args);

        act.Should().Throw<InvalidOperationException>().WithMessage("Validator not found.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateCompany_WhenValidArgs_ShouldCreateCompany()
    {
        var args = new CreateCompanyArgs(CompanyName, _validCurrentUser, CompanyRut, CompanyLogo, _validatorId);

        _companyRepositoryMock.Setup(cs => cs.Add(It.IsAny<Company>())).Verifiable();
        _userRepositoryMock.Setup(us => us.Update(It.IsAny<User>())).Verifiable();
        _validatorServiceMock.Setup(vs => vs.ValidatorIdIsValid(_validatorId)).Returns(true);

        _service.CreateCompany(args);

        _companyRepositoryMock.Verify(cs => cs.Add(It.Is<Company>(c =>
            c.Name == CompanyName &&
            c.Rut == CompanyRut &&
            c.Logo == CompanyLogo &&
            c.Owner.Id == _validCurrentUser.Id &&
            c.ValidatorId == _validatorId)), Times.Once);
        _companyRepositoryMock.VerifyAll();

        _validCurrentUser.HasCompany.Should().BeTrue();
        _userRepositoryMock.Verify(us => us.Update(It.Is<User>(u => u.HasCompany)), Times.Once);
        _userRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

    #region CreateCamera

    #region Error

    [TestMethod]
    public void CreateCamera_WhenUserIsNotCompanyOwner_ShouldThrowArgumentException()
    {
        var user = new User();
        var args = new CreateCameraWithoutCompanyArgs(DeviceName, Model, DeviceDescription,
            _validImages, CameraExternalUse, CameraInternalUse, CameraMotionDetection,
            CameraPersonDetection);

        Guid userId = user.Id;
        _companyRepositoryMock.Setup(cs => cs.Get(c => c.OwnerId == userId)).Returns(_validCompany);

        Action act = () => _service.CreateCamera(args, user);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("User is not the owner of the company.");
    }

    [TestMethod]
    public void CreateCamera_WhenCompanyNotFound_ShouldThrowArgumentException()
    {
        var args = new CreateCameraWithoutCompanyArgs(DeviceName, Model, DeviceDescription,
            _validImages, CameraExternalUse, CameraInternalUse, CameraMotionDetection,
            CameraPersonDetection);

        Guid userId = _validCurrentUser.Id;
        _companyRepositoryMock.Setup(cs => cs.Get(c => c.OwnerId == userId)).Returns((Company?)null);

        Action act = () => _service.CreateCamera(args, _validCurrentUser);

        act.Should().Throw<InvalidOperationException>().WithMessage("User does not have a company.");
    }

    [TestMethod]
    public void CreateCamera_WhenModelIsInvalid_ShouldThrowArgumentException()
    {
        var args = new CreateCameraWithoutCompanyArgs(DeviceName, Model, DeviceDescription,
            _validImages, CameraExternalUse, CameraInternalUse, CameraMotionDetection,
            CameraPersonDetection);
        Guid userId = _validCurrentUser.Id;

        _companyRepositoryMock.Setup(cs => cs.Get(c => c.OwnerId == userId)).Returns(_validCompany);
        _validatorServiceMock.Setup(vs => vs.IsValidModel(_validatorId, Model)).Returns(false);

        Action act = () => _service.CreateCamera(args, _validCurrentUser);

        act.Should().Throw<InvalidOperationException>().WithMessage("Model is not valid.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateCamera_WhenValidArgs_ShouldRegisterCamera()
    {
        var args = new CreateCameraWithoutCompanyArgs(DeviceName, Model, DeviceDescription,
            _validImages, CameraExternalUse, CameraInternalUse, CameraMotionDetection,
            CameraPersonDetection);
        Guid userId = _validCurrentUser.Id;

        _companyRepositoryMock.Setup(cs => cs.Get(c => c.OwnerId == userId)).Returns(_validCompany);
        _smartDeviceRepositoryMock.Setup(sd => sd.Add(It.IsAny<Camera>())).Verifiable();
        _validatorServiceMock.Setup(vs => vs.IsValidModel(_validatorId, Model)).Returns(true);
        _service.CreateCamera(args, _validCurrentUser);

        _smartDeviceRepositoryMock.Verify(sd => sd.Add(It.Is<Camera>(c =>
            c.Name == args.Name &&
            c.Model == args.Model &&
            c.Description == args.Description &&
            c.CompanyOwner.Id == _validCompany.Id &&
            c.HasExternalUse &&
            c.MotionDetection &&
            c.PersonDetection)), Times.Once);

        _smartDeviceRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

    #region CreateDevice

    #region Error

    [TestMethod]
    public void CreateMotionSensor_WhenUserIsNotCompanyOwner_ShouldThrowArgumentException()
    {
        var user = new User(
            new CreateUserArgs("John", "Doe", "doe@gmail.com", "Password--12", null)
            {
                Role = new Role("CompanyOwner")
            });
        var args = new CreateSmartDeviceWithoutCompanyArgs(DeviceName, Model,
            DeviceDescription, "MotionSensor", _validImages);
        Guid userId = user.Id;

        _companyRepositoryMock.Setup(cs => cs.Get(c => c.OwnerId == userId)).Returns(_validCompany);

        Action act = () => _service.CreateDevice(args, user);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("User is not the owner of the company.");
    }

    [TestMethod]
    public void CreateMotionSensor_WhenCompanyNotFound_ShouldThrowArgumentException()
    {
        var args = new CreateSmartDeviceWithoutCompanyArgs(DeviceName, Model, DeviceDescription, "MotionSensor",
            _validImages);
        Guid userId = _validCurrentUser.Id;

        _companyRepositoryMock.Setup(cs => cs.Get(c => c.OwnerId == userId)).Returns((Company?)null);

        Action act = () => _service.CreateDevice(args, _validCurrentUser);

        act.Should().Throw<InvalidOperationException>().WithMessage("User does not have a company.");
    }

    [TestMethod]
    public void CreateMotionSensor_WhenModelIsInvalid_ShouldThrowArgumentException()
    {
        var args = new CreateSmartDeviceWithoutCompanyArgs(DeviceName, Model,
            DeviceDescription, "MotionSensor", _validImages);
        Guid userId = _validCurrentUser.Id;

        _companyRepositoryMock.Setup(cs => cs.Get(c => c.OwnerId == userId)).Returns(_validCompany);
        _validatorServiceMock.Setup(vs => vs.IsValidModel(_validatorId, Model)).Returns(false);

        Action act = () => _service.CreateDevice(args, _validCurrentUser);

        act.Should().Throw<InvalidOperationException>().WithMessage("Model is not valid.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateMotionSensor_WhenValidArgs_ShouldRegisterSmartDevice()
    {
        var args = new CreateSmartDeviceWithoutCompanyArgs(DeviceName, Model, DeviceDescription, "MotionSensor",
            _validImages);
        Guid userId = _validCurrentUser.Id;

        _companyRepositoryMock.Setup(cs => cs.Get(c => c.OwnerId == userId)).Returns(_validCompany);
        _smartDeviceRepositoryMock.Setup(sd => sd.Add(It.IsAny<SmartDevice>())).Verifiable();
        _validatorServiceMock.Setup(vs => vs.IsValidModel(_validatorId, Model)).Returns(true);

        _service.CreateDevice(args, _validCurrentUser);

        _smartDeviceRepositoryMock.Verify(sd => sd.Add(It.Is<SmartDevice>(d =>
            d.Name == args.Name &&
            d.Model == args.Model &&
            d.Description == args.Description &&
            d.CompanyOwner.Id == _validCompany.Id)), Times.Once);
        _smartDeviceRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion
}
