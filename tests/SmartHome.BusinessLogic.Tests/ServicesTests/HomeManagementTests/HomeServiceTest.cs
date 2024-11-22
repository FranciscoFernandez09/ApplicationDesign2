using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.EFCoreClasses;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.BusinessLogic.Services.HomeManagement;

namespace SmartHome.BusinessLogic.Tests.ServicesTests.HomeManagementTests;

[TestClass]
public class HomeServiceTest
{
    private const string AddressStreet = "ValidAddressStreet";
    private const int AddressNumber = 123;
    private const int Latitude = 10;
    private const int Longitude = 99;
    private const int MaxMembers = 4;
    private const string HomeName = "name";
    private const string Type = "MotionSensor";
    private Mock<IRepository<HomeDevice>> _homeDeviceRepositoryMock = null!;
    private Mock<IRepository<HomeMember>> _homeMemberRepositoryMock = null!;
    private Mock<IRepository<HomePermission>> _homePermissionRepositoryMock = null!;
    private Mock<IRepository<Home>> _homeRepositoryMock = null!;
    private Mock<IRepository<MemberHomePermission>> _memberHomePermissionMock = null!;

    private HomeService _service = null!;
    private Mock<IRepository<SmartDevice>> _smartDeviceRepositoryMock = null!;
    private Mock<IRepository<User>> _userRepositoryMock = null!;
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

        _userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Strict);
        _homeRepositoryMock = new Mock<IRepository<Home>>(MockBehavior.Strict);
        _homeMemberRepositoryMock = new Mock<IRepository<HomeMember>>(MockBehavior.Strict);
        _smartDeviceRepositoryMock = new Mock<IRepository<SmartDevice>>(MockBehavior.Strict);
        _homeDeviceRepositoryMock = new Mock<IRepository<HomeDevice>>(MockBehavior.Strict);
        _homePermissionRepositoryMock = new Mock<IRepository<HomePermission>>(MockBehavior.Strict);
        _memberHomePermissionMock = new Mock<IRepository<MemberHomePermission>>(MockBehavior.Strict);

        _service = new HomeService(_userRepositoryMock.Object, _homeRepositoryMock.Object,
            _smartDeviceRepositoryMock.Object, _homeDeviceRepositoryMock.Object, _homeMemberRepositoryMock.Object,
            _homePermissionRepositoryMock.Object, _memberHomePermissionMock.Object);

        _validCurrentUser =
            new User(new CreateUserArgs(name, lastName, email, password, profileImage)
            {
                Role = new Role { Id = Constant.HomeOwnerRoleId, Name = "HomeOwner" }
            });

        _validHome = new Home(new CreateHomeArgs(AddressStreet, AddressNumber, Latitude, Longitude,
            MaxMembers, HomeName, _validCurrentUser));

        var imageList = new List<DeviceImage> { new("logo.png", true) };
        _validCompany = new Company(new CreateCompanyArgs("company", _validCurrentUser, "1234567890-1", "logo.png",
            Guid.Parse("f3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b")));
        _validSmartDevice =
            new SmartDevice(
                new CreateSmartDeviceArgs("Device", "AAA111", "Description", _validCompany, Type, imageList));
    }

    #region CreateHome

    [TestMethod]
    public void CreateHome_WhenParametersAreValid_ShouldCreateHome()
    {
        var args = new CreateHomeArgs(AddressStreet, AddressNumber, Latitude, Longitude,
            MaxMembers, HomeName, _validCurrentUser);

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == _validCurrentUser.Id)).Returns(_validCurrentUser);
        _homeRepositoryMock.Setup(hs => hs.Add(It.IsAny<Home>())).Verifiable();
        _homeMemberRepositoryMock.Setup(hms => hms.Add(It.IsAny<HomeMember>())).Verifiable();

        _service.CreateHome(args);

        _homeRepositoryMock.Verify(hs => hs.Add(It.Is<Home>(h =>
            h.AddressStreet == AddressStreet &&
            h.AddressNumber == AddressNumber &&
            h.Longitude == Longitude &&
            h.Latitude == Latitude &&
            h.MaxMembers == MaxMembers &&
            h.Id != Guid.Empty)), Times.Once);
    }

    #endregion

    #region AddMember

    #region Error

    [TestMethod]
    public void AddMember_WhenHomeIdIsNull_ShouldThrowArgumentNullException()
    {
        var memberEmail = _validCurrentUser.Email;

        Action act = () => _service.AddMember(_validCurrentUser, memberEmail, null);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'homeId')");
    }

    [TestMethod]
    public void AddMember_WhenHomeIdIsEmpty_ShouldThrowArgumentNullException()
    {
        var memberEmail = _validCurrentUser.Email;

        Action act = () => _service.AddMember(_validCurrentUser, memberEmail, Guid.Empty);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'homeId')");
    }

    [TestMethod]
    public void AddMember_WhenUserIdIsNotFound_ShouldThrowInvalidOperationException()
    {
        Guid? currentUserId = _validCurrentUser.Id;
        var memberEmail = _validCurrentUser.Email;
        Guid? homeId = Guid.NewGuid();

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == currentUserId)).Returns(_validCurrentUser);
        _userRepositoryMock.Setup(us => us.Get(u => u.Email == memberEmail)).Returns((User?)null);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(_validHome);

        Action act = () => _service.AddMember(_validCurrentUser, memberEmail, homeId);

        act.Should().Throw<InvalidOperationException>().WithMessage("User not found.");
    }

    [TestMethod]
    public void AddMember_WhenHomeIdIsNotFound_ShouldThrowInvalidOperationException()
    {
        var memberEmail = _validCurrentUser.Email;
        Guid? homeId = Guid.NewGuid();

        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns((Home?)null);

        Action act = () => _service.AddMember(_validCurrentUser, memberEmail, homeId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Home not found.");
    }

    [TestMethod]
    public void AddMember_WhenMemberEmailIsNull_ShouldThrowArgumentNullException()
    {
        Guid? homeId = _validHome.Id;

        Action act = () => _service.AddMember(_validCurrentUser, null, homeId);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'memberEmail')");
    }

    [TestMethod]
    public void AddMember_WhenHomeMemberCapacityIsFull_ShouldThrowInvalidOperationException()
    {
        const int maxMembers = 1;
        var home = new Home(new CreateHomeArgs(AddressStreet, AddressNumber, Latitude, Longitude,
            maxMembers, HomeName, _validCurrentUser));
        var memberEmail = _validCurrentUser.Email;
        Guid? homeId = home.Id;

        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(home);

        Action act = () => _service.AddMember(_validCurrentUser, memberEmail, homeId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Home member capacity is full.");
    }

    [TestMethod]
    public void AddMember_WhenUserIsAlreadyMember_ShouldThrowInvalidOperationException()
    {
        var member =
            new User(new CreateUserArgs("John", "Doe", "john@gmail.com", "Password-123", "photo.jpg")
            {
                Role = new Role("HomeOwner")
            });
        Guid? currentUserId = _validCurrentUser.Id;
        var memberEmail = _validCurrentUser.Email;
        Guid? homeId = _validHome.Id;

        var homeUser = new HomeMember(_validHome, member);
        _validHome.AddMember(homeUser);

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == currentUserId)).Returns(_validCurrentUser);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(_validHome);
        _userRepositoryMock.Setup(us => us.Get(u => u.Email == memberEmail)).Returns(member);

        Action act = () => _service.AddMember(_validCurrentUser, memberEmail, homeId);
        act.Should().Throw<InvalidOperationException>().WithMessage("User is already a member of the home.");
    }

    [TestMethod]
    public void AddMember_WhenUserNotIsHomeOwnerRole_ShouldThrowInvalidOperationException()
    {
        var invalidMember =
            new User(new CreateUserArgs("John", "Doe", "john@gmail.com", "Password-123", "photo.jpg")
            {
                Role = new Role("AnotherRol")
            });
        var currentUserEmail = _validCurrentUser.Email;
        var memberEmail = invalidMember.Email;
        Guid? homeId = _validHome.Id;

        _userRepositoryMock.Setup(us => us.Get(u => u.Email == currentUserEmail)).Returns(_validCurrentUser);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(_validHome);
        _userRepositoryMock.Setup(us => us.Get(u => u.Email == memberEmail)).Returns(invalidMember);

        Action act = () => _service.AddMember(_validCurrentUser, memberEmail, homeId);
        act.Should().Throw<InvalidOperationException>().WithMessage("User does not have owner home role.");
    }

    [TestMethod]
    public void AddMember_WhenCurrentUserIsNotHomeOwner_ShouldThrowUnauthorizedAccessException()
    {
        var member =
            new User(new CreateUserArgs("John", "Doe", "john@gmail.com", "Password-123", "photo.jpg")
            {
                Role = new Role("HomeOwner")
            });
        var memberEmail = member.Email;
        Guid? homeId = _validHome.Id;

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == member.Id)).Returns(member);
        _userRepositoryMock.Setup(us => us.Get(u => u.Email == memberEmail)).Returns(member);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(_validHome);

        Action act = () => _service.AddMember(member, memberEmail, homeId);

        act.Should().Throw<UnauthorizedAccessException>().WithMessage("User does not have permission to add member.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void AddMember_WhenParametersAreValid_ShouldAddMember()
    {
        var previousCantMembers = _validHome.MembersCount;
        var member =
            new User(new CreateUserArgs("John", "Doe", "john@gmail.com", "Password-123", "photo.jpg")
            {
                Role = new Role { Id = Constant.HomeOwnerRoleId, Name = "HomeOwner" }
            });
        var memberEmail = member.Email;
        Guid? homeId = _validHome.Id;

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == _validCurrentUser.Id)).Returns(_validCurrentUser);
        _userRepositoryMock.Setup(us => us.Get(u => u.Email == memberEmail)).Returns(member);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(_validHome);
        _homeMemberRepositoryMock.Setup(hms => hms.Add(It.IsAny<HomeMember>()))
            .Callback<HomeMember>(hm => _validHome.AddMember(hm)).Verifiable();

        _service.AddMember(_validCurrentUser, memberEmail, homeId);

        _validHome.MembersCount.Should().Be(previousCantMembers + 1);
    }

    [TestMethod]
    public void AddMember_WhenMemberIsAdminHomeOwner_ShouldAddMember()
    {
        var previousCantMembers = _validHome.MembersCount;
        var member =
            new User(new CreateUserArgs("John", "Doe", "john@gmail.com", "Password-123", "photo.jpg")
            {
                Role = new Role { Id = Constant.AdminHomeOwnerRoleId, Name = "AdminHomeOwner" }
            });
        var memberEmail = member.Email;
        Guid? homeId = _validHome.Id;

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == _validCurrentUser.Id)).Returns(_validCurrentUser);
        _userRepositoryMock.Setup(us => us.Get(u => u.Email == memberEmail)).Returns(member);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(_validHome);
        _homeMemberRepositoryMock.Setup(hms => hms.Add(It.IsAny<HomeMember>()))
            .Callback<HomeMember>(hm => _validHome.AddMember(hm)).Verifiable();

        _service.AddMember(_validCurrentUser, memberEmail, homeId);

        _validHome.MembersCount.Should().Be(previousCantMembers + 1);
    }

    [TestMethod]
    public void AddMember_WhenMemberIsCompanyAndHomeOwner_ShouldAddMember()
    {
        var previousCantMembers = _validHome.MembersCount;
        var member =
            new User(new CreateUserArgs("John", "Doe", "john@gmail.com", "Password-123", "photo.jpg")
            {
                Role = new Role { Id = Constant.CompanyAndHomeOwnerRoleId, Name = "CompanyAndHomeOwner" }
            });
        var memberEmail = member.Email;
        Guid? homeId = _validHome.Id;

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == _validCurrentUser.Id)).Returns(_validCurrentUser);
        _userRepositoryMock.Setup(us => us.Get(u => u.Email == memberEmail)).Returns(member);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(_validHome);
        _homeMemberRepositoryMock.Setup(hms => hms.Add(It.IsAny<HomeMember>()))
            .Callback<HomeMember>(hm => _validHome.AddMember(hm)).Verifiable();

        _service.AddMember(_validCurrentUser, memberEmail, homeId);

        _validHome.MembersCount.Should().Be(previousCantMembers + 1);
    }

    #endregion

    #endregion

    #region AddHomePermission

    #region Error

    [TestMethod]
    public void AddHomePermission_WhenMemberNotFound_ShouldThrowInvalidOperationException()
    {
        Guid? permission = Constant.GetHomeDevicesId;
        Guid? userId = Guid.NewGuid();
        var args = new AddHomePermissionArgs(_validCurrentUser, userId, permission);

        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.Id == args.MemberId)).Returns((HomeMember?)null);

        Action act = () => _service.AddHomePermission(args);

        act.Should().Throw<InvalidOperationException>().WithMessage("Member not found.");
    }

    [TestMethod]
    public void AddHomePermission_WhenCurrentUserNotIsOwner_ShouldThrowUnauthorizedAccessException()
    {
        var member =
            new User(new CreateUserArgs("member", "member", "member@gmail.com", "AAaa//**1", "image.jpg")
            {
                Role = new Role("HomeOwner")
            });
        var standardUser =
            new User(new CreateUserArgs("John", "Doe", "JohnDoe@gmail.com", "Password-123", "profileImage.png")
            {
                Role = new Role("HomeOwner")
            });
        var homeMember = new HomeMember(_validHome, standardUser);

        Guid? permission = Constant.GetHomeDevicesId;
        Guid? memberId = member.Id;
        var args = new AddHomePermissionArgs(standardUser, memberId, permission);

        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.Id == args.MemberId)).Returns(homeMember);

        Action act = () => _service.AddHomePermission(args);

        act.Should().Throw<UnauthorizedAccessException>()
            .WithMessage("User does not have permission to add member permission.");
    }

    [TestMethod]
    public void AddHomePermission_WhenUserAlreadyHasPermission_ShouldThrowInvalidOperationException()
    {
        var user = new User(
            new CreateUserArgs("John", "Doe", "JohnDoe@gmail.com", "Password-123", "profileImage.png")
            {
                Role = new Role("HomeOwner")
            });

        Guid? permissionId = Constant.GetHomeDevicesId;
        var homeMember = new HomeMember(_validHome, user);
        _validHome.AddMember(homeMember);

        Guid? userId = user.Id;
        var args = new AddHomePermissionArgs(_validCurrentUser, userId, permissionId);

        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.Id == args.MemberId)).Returns(homeMember);
        _memberHomePermissionMock.Setup(mps =>
            mps.Exists(mhp => mhp.MemberId == args.MemberId && mhp.PermissionId == args.PermissionId)).Returns(true);

        Action act = () => _service.AddHomePermission(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("User already has this permission.");
    }

    [TestMethod]
    public void AddHomePermission_WhenUserIsOwner_ShouldThrowInvalidOperationException()
    {
        Guid? permissionId = Constant.GetHomeDevicesId;
        Guid? userId = _validCurrentUser.Id;
        var homeMember = new HomeMember(_validHome, _validCurrentUser);
        var args = new AddHomePermissionArgs(_validCurrentUser, userId, permissionId);

        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.Id == args.MemberId)).Returns(homeMember);
        _memberHomePermissionMock.Setup(mps =>
            mps.Exists(mhp => mhp.MemberId == args.MemberId && mhp.PermissionId == args.PermissionId)).Returns(false);

        Action act = () => _service.AddHomePermission(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("User already has this permission.");
    }

    [TestMethod]
    public void AddHomePermission_WhenPermissionIsNotFound_ShouldThrowInvalidOperationException()
    {
        var user = new User(
            new CreateUserArgs("John", "Doe", "JohnDoe@gmail.com", "Password-123", "profileImage.png")
            {
                Role = new Role("HomeOwner")
            });
        var homeMember = new HomeMember(_validHome, user);
        _validHome.AddMember(homeMember);

        Guid permission = Constant.GetHomeDevicesId;
        Guid? userId = user.Id;
        var args = new AddHomePermissionArgs(_validCurrentUser, userId, permission);

        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.Id == args.MemberId)).Returns(homeMember);
        _memberHomePermissionMock.Setup(mps =>
            mps.Exists(mhp => mhp.MemberId == args.MemberId && mhp.PermissionId == args.PermissionId)).Returns(false);
        _homePermissionRepositoryMock.Setup(hps => hps.Get(hp => hp.Id == args.PermissionId))
            .Returns((HomePermission?)null);

        Action act = () => _service.AddHomePermission(args);

        act.Should().Throw<InvalidOperationException>().WithMessage("Permission not found.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void AddHomePermission_WhenParametersAreValid_ShouldAddPermission()
    {
        var user = new User(
            new CreateUserArgs("John", "Doe", "JohnDoe@gmail.com", "Password-123", "profileImage.png")
            {
                Role = new Role("HomeOwner")
            });
        Home home = _validHome;
        var homeMember = new HomeMember(home, user);
        home.AddMember(homeMember);
        var args = new AddHomePermissionArgs(_validCurrentUser, user.Id, Constant.AddHomePermissionId);
        var homePermission = new HomePermission { Id = Constant.AddHomePermissionId, Name = "AddHomePermission" };

        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.Id == args.MemberId)).Returns(homeMember);
        _homePermissionRepositoryMock.Setup(hps => hps.Get(hp => hp.Id == args.PermissionId))
            .Returns(homePermission);
        _memberHomePermissionMock.Setup(mps =>
            mps.Exists(mhp => mhp.MemberId == args.MemberId && mhp.PermissionId == args.PermissionId)).Returns(false);
        _memberHomePermissionMock.Setup(mhp => mhp.Add(It.IsAny<MemberHomePermission>()))
            .Callback<MemberHomePermission>(mhp => homeMember.AddHomePermission(mhp.Permission)).Verifiable();

        _service.AddHomePermission(args);

        homeMember.HasHomePermission(homePermission.Id).Should().BeTrue();
    }

    #endregion

    #endregion

    #region AddDevice

    #region Error

    [TestMethod]
    public void AddSmartDevice_WhenHomeIdIsNull_ShouldThrowArgumentNullException()
    {
        var deviceId = Guid.NewGuid();
        Action act = () => _service.AddSmartDevice(_validCurrentUser, null, deviceId);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'homeId')");
    }

    [TestMethod]
    public void AddSmartDevice_WhenDeviceIdIsNull_ShouldThrowArgumentNullException()
    {
        var homeId = Guid.NewGuid();
        Action act = () => _service.AddSmartDevice(_validCurrentUser, homeId, null);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'deviceId')");
    }

    [TestMethod]
    public void AddSmartDevice_WhenDeviceIdIsEmpty_ShouldThrowArgumentNullException()
    {
        var homeId = Guid.NewGuid();
        Action act = () => _service.AddSmartDevice(_validCurrentUser, homeId, Guid.Empty);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'deviceId')");
    }

    [TestMethod]
    public void AddSmartDevice_WhenHomeIdIsEmpty_ShouldThrowArgumentNullException()
    {
        var deviceId = Guid.NewGuid();
        Action act = () => _service.AddSmartDevice(_validCurrentUser, Guid.Empty, deviceId);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'homeId')");
    }

    [TestMethod]
    public void AddSmartDevice_WhenCurrentUserIsNotHomeMember_ShouldThrowUnauthorizedAccessException()
    {
        var role = new Role("HomeOwner");
        role.AddPermission(new SystemPermission("AddSmartDevice"));
        var user = new User(
            new CreateUserArgs("John", "Doe", "JohnDoe@gmail.com", "Password-123", "profileImage.png")
            {
                Role = new Role("HomeOwner")
            });
        Home home = _validHome;
        Guid? homeId = home.Id;

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == user.Id)).Returns(user);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(home);
        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm =>
            hm.HomeId == homeId && hm.UserId == user.Id)).Returns((HomeMember?)null);

        Action act = () => _service.AddSmartDevice(user, homeId, Guid.NewGuid());

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("User is not home member.");
    }

    [TestMethod]
    public void AddSmartDevice_WhenHomeIdIsNotFound_ShouldThrowInvalidOperationException()
    {
        Guid? deviceId = Guid.NewGuid();
        Home home = _validHome;
        Guid? homeId = home.Id;
        User user = _validCurrentUser;
        var homeMember = new HomeMember(home, user);

        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.HomeId == homeId && hm.UserId == user.Id))
            .Returns(homeMember);
        _userRepositoryMock.Setup(us => us.Get(u => u.Id == _validCurrentUser.Id)).Returns(_validCurrentUser);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns((Home?)null);

        Action act = () => _service.AddSmartDevice(_validCurrentUser, homeId, deviceId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Home not found.");
    }

    [TestMethod]
    public void AddSmartDevice_WhenUserHasNoHomePermission_ShouldThrowUnauthorizedAccessException()
    {
        var user = new User(
            new CreateUserArgs("John", "Doe", "JohnDoe@gmail.com", "Password-123", "profileImage.png")
            {
                Role = new Role("HomeOwner")
            });
        Home home = _validHome;
        Guid? homeId = home.Id;
        var homeMember = new HomeMember(home, user);

        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.HomeId == homeId && hm.UserId == user.Id))
            .Returns(homeMember);
        _userRepositoryMock.Setup(us => us.Get(u => u.Id == user.Id)).Returns(user);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(home);

        Action act = () => _service.AddSmartDevice(user, homeId, Guid.NewGuid());

        act.Should().Throw<UnauthorizedAccessException>()
            .WithMessage("User does not have permission to add smart device.");
    }

    [TestMethod]
    public void AddSmartDevice_WhenDeviceIsNotFound_ShouldThrowInvalidOperationException()
    {
        Home home = _validHome;
        User user = _validCurrentUser;
        Guid? deviceId = _validSmartDevice.Id;
        Guid? homeId = home.Id;
        var homeMember = new HomeMember(home, user);
        var homePermission = new HomePermission { Id = Constant.AddSmartDeviceId, Name = "AddSmartDevice" };
        homeMember.AddHomePermission(homePermission);

        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.HomeId == homeId && hm.UserId == user.Id))
            .Returns(homeMember);
        _userRepositoryMock.Setup(us => us.Get(u => u.Id == _validCurrentUser.Id)).Returns(_validCurrentUser);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(_validHome);
        _smartDeviceRepositoryMock.Setup(ds => ds.Get(d => d.Id == deviceId)).Returns((SmartDevice?)null);

        Action act = () => _service.AddSmartDevice(_validCurrentUser, homeId, deviceId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device not found.");
    }

    [TestMethod]
    public void AddSmartDevice_WhenDeviceAlreadyIsInHome_ShouldThrowInvalidOperationException()
    {
        Home home = _validHome;
        User user = _validCurrentUser;
        Guid? deviceId = _validSmartDevice.Id;
        Guid? homeId = home.Id;
        var homeMember = new HomeMember(home, user);
        var homePermission = new HomePermission { Id = Constant.AddSmartDeviceId, Name = "AddSmartDevice" };
        homeMember.AddHomePermission(homePermission);

        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.HomeId == homeId && hm.UserId == user.Id))
            .Returns(homeMember);
        _userRepositoryMock.Setup(us => us.Get(u => u.Id == _validCurrentUser.Id)).Returns(_validCurrentUser);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(_validHome);
        _smartDeviceRepositoryMock.Setup(ds => ds.Get(d => d.Id == deviceId)).Returns(_validSmartDevice);
        _homeDeviceRepositoryMock.Setup(hds => hds.Exists(hd => hd.DeviceId == deviceId && hd.HomeId == home.Id))
            .Returns(true);

        Action act = () => _service.AddSmartDevice(_validCurrentUser, homeId, deviceId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device is already added in the home.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void AddSmartDevice_WhenParametersAreValid_ShouldAddDevice()
    {
        User user = _validCurrentUser;
        Home home = _validHome;
        var homeMember = new HomeMember(home, user);
        var homePermission = new HomePermission { Id = Constant.AddSmartDeviceId, Name = "AddSmartDevice" };
        homeMember.AddHomePermission(homePermission);
        SmartDevice smartDevice = _validSmartDevice;
        Guid? deviceId = smartDevice.Id;
        Guid? homeId = home.Id;

        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.HomeId == homeId && hm.UserId == user.Id))
            .Returns(homeMember);
        _userRepositoryMock.Setup(us => us.Get(u => u.Id == user.Id)).Returns(user);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(home);
        _smartDeviceRepositoryMock.Setup(ds => ds.Get(d => d.Id == deviceId)).Returns(smartDevice);
        _homeDeviceRepositoryMock.Setup(ds => ds.Exists(hd => hd.DeviceId == deviceId && hd.HomeId == home.Id))
            .Returns(false);
        _homeDeviceRepositoryMock.Setup(hds => hds.Add(It.IsAny<HomeDevice>())).Verifiable();

        _service.AddSmartDevice(user, homeId, deviceId);

        _homeDeviceRepositoryMock.Verify(hds => hds.Add(It.Is<HomeDevice>(hd =>
            hd.Id != Guid.Empty &&
            hd.HomeId == homeId &&
            hd.Home == home &&
            hd.DeviceId == deviceId &&
            hd.Device == smartDevice &&
            hd.IsConnected == true)), Times.Once);
    }

    #endregion

    #endregion

    #region GetHomeMembers

    #region Error

    [TestMethod]
    public void GetHomeMembers_WhenHomeIdIsNull_ShouldThrowArgumentNullException()
    {
        Func<List<ShowHomeMemberDto>> act = () => _service.GetHomeMembers(_validCurrentUser, null);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'homeId')");
    }

    [TestMethod]
    public void GetHomeMembers_WhenHomeIdIsEmpty_ShouldThrowArgumentNullException()
    {
        Func<List<ShowHomeMemberDto>> act = () => _service.GetHomeMembers(_validCurrentUser, Guid.Empty);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'homeId')");
    }

    [TestMethod]
    public void GetHomeMembers_WhenHomeIsNotFound_ShouldThrowInvalidOperationException()
    {
        Guid? currentUserId = _validCurrentUser.Id;
        Guid? homeId = Guid.NewGuid();

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == currentUserId)).Returns(_validCurrentUser);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns((Home?)null);

        Func<List<ShowHomeMemberDto>> act = () => _service.GetHomeMembers(_validCurrentUser, homeId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Home not found.");
    }

    [TestMethod]
    public void GetHomeMembers_WhenCurrentUserIsNotHomeOwnerOrMember_ShouldThrowUnauthorizedAccessException()
    {
        var role = new Role("HomeOwner");
        role.AddPermission(new SystemPermission("GetHomeMembers"));
        var user = new User(
            new CreateUserArgs("John", "Doe", "JohnDoe@gmail.com", "Password-123", "profileImage.png")
            {
                Role = new Role("HomeOwner")
            });

        Guid? userId = user.Id;
        Guid? homeId = _validHome.Id;

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == userId)).Returns(user);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(_validHome);

        Func<List<ShowHomeMemberDto>> act = () => _service.GetHomeMembers(user, homeId);

        act.Should().Throw<UnauthorizedAccessException>()
            .WithMessage("User does not have permission to get home members.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetHomeMembers_WhenParametersAreValid_ShouldReturnHomeMembers()
    {
        Home home = _validHome;
        var member = new HomeMember(home, _validCurrentUser);
        home.AddMember(member);

        Guid? currentUserId = _validCurrentUser.Id;
        Guid? homeId = home.Id;
        List<HomeMember> members = [member];
        var userData = member.GetUserData();
        List<ShowHomeMemberDto> expected =
        [
            new ShowHomeMemberDto(member.Id, userData[0], userData[1], userData[2], member.ShouldNotify,
                userData[3])
        ];

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == currentUserId)).Returns(_validCurrentUser);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(home);
        _homeMemberRepositoryMock.Setup(hms => hms.GetAll(hm => hm.HomeId == homeId)).Returns(members);

        List<ShowHomeMemberDto> result = _service.GetHomeMembers(_validCurrentUser, homeId);

        result.Should().BeEquivalentTo(expected);
    }

    #endregion

    #endregion

    #region GetHomeDevices

    #region Error

    [TestMethod]
    public void GetDevices_WhenHomeIdIsNull_ShouldThrowArgumentNullException()
    {
        Guid? currentId = _validCurrentUser.Id;
        Guid? roomId = Guid.NewGuid();

        Func<List<ShowHomeDeviceDto>> act = () => _service.GetHomeDevices(_validCurrentUser, null, roomId);

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == currentId)).Returns(_validCurrentUser);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'homeId')");
    }

    [TestMethod]
    public void GetDevices_WhenHomeIsNotFound_ShouldThrowInvalidOperationException()
    {
        Guid? currentUserId = _validCurrentUser.Id;
        Guid? homeId = Guid.NewGuid();
        Guid? roomId = Guid.NewGuid();

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == currentUserId)).Returns(_validCurrentUser);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns((Home?)null);

        Func<List<ShowHomeDeviceDto>> act = () => _service.GetHomeDevices(_validCurrentUser, homeId, roomId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Home not found.");
    }

    [TestMethod]
    public void GetDevices_WhenCurrentUserIsMemberAndHasNotHomePermission_ShouldThrowUnauthorizedAccessException()
    {
        var currentUser =
            new User(new CreateUserArgs("John", "Doe", "john@gmail.com", "Password-123", string.Empty)
            {
                Role = new Role("HomeOwner")
            });
        Home home = _validHome;
        var member = new HomeMember(_validHome, currentUser);
        _validHome.AddMember(member);

        Guid? currentUserId = currentUser.Id;
        Guid? homeId = _validHome.Id;
        Guid? roomId = Guid.NewGuid();

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == currentUserId)).Returns(currentUser);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(_validHome);
        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm =>
            hm.HomeId == home.Id && hm.UserId == currentUser.Id)).Returns(member);

        Func<List<ShowHomeDeviceDto>> act = () => _service.GetHomeDevices(currentUser, homeId, roomId);

        act.Should().Throw<UnauthorizedAccessException>()
            .WithMessage("User does not have permission to get home devices.");
    }

    [TestMethod]
    public void GetDevices_WhenCurrentUserIsNotHomeOwnerOrMember_ShouldThrowUnauthorizedAccessException()
    {
        var role = new Role("HomeOwner");
        role.AddPermission(new SystemPermission("AddSmartDevice"));
        var standardUser =
            new User(new CreateUserArgs("John", "Doe", "john@gmail.com", "Password-123", string.Empty) { Role = role });
        Home home = _validHome;

        Guid? currentUserId = standardUser.Id;
        Guid? homeId = _validHome.Id;
        Guid? roomId = Guid.NewGuid();

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == currentUserId)).Returns(standardUser);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(_validHome);
        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm =>
            hm.HomeId == home.Id && hm.UserId == standardUser.Id)).Returns((HomeMember?)null);

        Func<List<ShowHomeDeviceDto>> act = () => _service.GetHomeDevices(standardUser, homeId, roomId);

        act.Should().Throw<UnauthorizedAccessException>()
            .WithMessage("User does not have permission to get home devices.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetDevices_WhenParametersAreValid_ShouldReturnHomeDevices()
    {
        var member = new HomeMember(_validHome, _validCurrentUser);
        var device = new HomeDevice(_validHome, _validSmartDevice);
        List<HomeDevice> devices = [device];

        Guid? currentUserId = _validCurrentUser.Id;
        Guid? homeId = _validHome.Id;
        Guid? roomId = Guid.NewGuid();
        var deviceData = device.GetNameAndMainImage();
        List<ShowHomeDeviceDto> expected =
        [
            new ShowHomeDeviceDto(device.Id, device.Name!, Type, false, device.GetDeviceModel(), deviceData[1],
                device.IsConnected)
        ];
        Home home = _validHome;
        User user = _validCurrentUser;
        var roomIdIsNullOrEmpty = roomId == Guid.Empty;

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == currentUserId)).Returns(user);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(home);
        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.HomeId == home.Id && hm.UserId == user.Id))
            .Returns(member);
        _homeDeviceRepositoryMock.Setup(hds => hds.GetAll(hd =>
                hd.HomeId == homeId && (roomIdIsNullOrEmpty || (hd.Room != null && hd.Room.Id == roomId))))
            .Returns(devices);

        List<ShowHomeDeviceDto> result = _service.GetHomeDevices(user, homeId, roomId);

        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetHomeDevices_WhenCurrentIsMemberWithPermission_ShouldReturnHomeDevices()
    {
        var role = new Role("HomeOwner");
        role.AddPermission(new SystemPermission { Id = Constant.GetHomeDevicesId, Name = "GetHomeDevices" });
        var user = new User
        {
            Name = "John",
            LastName = "Doe",
            Email = "doe@gmail.com",
            Password = "Password-123",
            Role = role
        };
        Home home = _validHome;
        var member = new HomeMember(home, user);
        member.AddHomePermission(new HomePermission { Id = Constant.GetHomeDevicesId, Name = "GetHomeDevices" });

        Guid? currentUserId = user.Id;
        Guid? homeId = home.Id;
        Guid? roomId = Guid.NewGuid();
        var device = new HomeDevice(_validHome, _validSmartDevice);
        List<HomeDevice> devices = [device];
        _validHome.AddMember(member);
        _validHome.AddDevice(device);

        var deviceData = device.GetNameAndMainImage();
        List<ShowHomeDeviceDto> expected =
        [
            new ShowHomeDeviceDto(device.Id, device.Name!, Type, false, device.GetDeviceModel(), deviceData[1],
                device.IsConnected)
        ];
        var roomIdIsNullOrEmpty = roomId == Guid.Empty;

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == currentUserId)).Returns(user);
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(home);
        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.HomeId == home.Id && hm.UserId == user.Id))
            .Returns(member);
        _homeDeviceRepositoryMock.Setup(hds => hds.GetAll(hd =>
                hd.HomeId == homeId && (roomIdIsNullOrEmpty || (hd.Room != null && hd.Room.Id == roomId))))
            .Returns(devices);

        List<ShowHomeDeviceDto> result = _service.GetHomeDevices(user, homeId, roomId);

        result.Should().BeEquivalentTo(expected);
    }

    #endregion

    #endregion

    #region ModifyHomeName

    #region Error

    [TestMethod]
    public void ModifyHomeName_WhenHomeIdIsNullOrEmpty_ShouldReturnArgumentNullException()
    {
        Action act = () => _service.ModifyHomeName(_validCurrentUser, null, "New Home Name");

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'homeId')");
    }

    [TestMethod]
    public void ModifyHomeName_WhenHomeNameIsNull_ShouldReturnArgumentNullException()
    {
        Action act = () => _service.ModifyHomeName(_validCurrentUser, Guid.NewGuid(), null);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'name')");
    }

    [TestMethod]
    public void ModifyHomeName_WhenHomeNameIsEmpty_ShouldReturnInvalidOperationException()
    {
        Action act = () => _service.ModifyHomeName(_validCurrentUser, Guid.NewGuid(), string.Empty);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'name')");
    }

    [TestMethod]
    public void ModifyHomeName_WhenHomeIsNotFound_ShouldReturnInvalidOperationException()
    {
        Home home = _validHome;
        Guid? homeId = home.Id;

        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns((Home?)null);

        Action act = () => _service.ModifyHomeName(_validCurrentUser, homeId, "New Home Name");

        act.Should().Throw<InvalidOperationException>().WithMessage("Home not found.");
    }

    [TestMethod]
    public void ModifyHomeName_WhenUserIsNotHomeMember_ShouldReturnUnauthorizedAccessException()
    {
        var user = new User(
            new CreateUserArgs("John", "Doe", "JohnDoe@gmail.com", "Password-123", "profileImage.png")
            {
                Role = new Role("HomeOwner")
            });
        Home home = _validHome;
        Guid? homeId = home.Id;

        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(home);
        _userRepositoryMock.Setup(us => us.Get(u => u.Id == user.Id)).Returns(user);

        Action act = () => _service.ModifyHomeName(user, homeId, "New Home Name");

        act.Should().Throw<UnauthorizedAccessException>()
            .WithMessage("User does not is member of the home.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void ModifyHomeName_WhenParametersAreValid_ShouldModifyHomeName()
    {
        Home home = _validHome;
        Guid? homeId = home.Id;
        var newHomeName = "New Home Name";
        User user = _validCurrentUser;

        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(home);
        _homeRepositoryMock.Setup(hs => hs.Update(It.IsAny<Home>())).Verifiable();
        _userRepositoryMock.Setup(us => us.Get(u => u.Id == user.Id)).Returns(user);
        _service.ModifyHomeName(_validCurrentUser, homeId, newHomeName);

        home.Name.Should().Be(newHomeName);
        _homeRepositoryMock.Verify(hs => hs.Update(It.Is<Home>(h => h.Name == newHomeName)), Times.Once);
    }

    #endregion

    #endregion

    #region GetMineHomes

    [TestMethod]
    public void GetMineHomes_WhenUserHasHomes_ShouldReturnListOfShowHomeDto()
    {
        User user = _validCurrentUser;
        var args = new CreateHomeArgs(AddressStreet, AddressNumber, Latitude, Longitude, MaxMembers, HomeName, user);
        var home = new Home(args);
        var expected = new List<ShowHomeDto> { new(home.Id, home.Name) };

        _homeRepositoryMock.Setup(hs => hs.GetAll(h => h.Owner.Id == user.Id)).Returns([home]);

        List<ShowHomeDto> result = _service.GetMineHomes(user);

        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetMineHomes_WhenUserHasNoHomes_ShouldReturnEmptyList()
    {
        User user = _validCurrentUser;
        var home = new List<Home>();

        _homeRepositoryMock.Setup(hs => hs.GetAll(h => h.Owner.Id == user.Id)).Returns(home);

        List<ShowHomeDto> result = _service.GetMineHomes(user);

        result.Should().BeEmpty();
    }

    #endregion

    #region GetHomesWhereIMember

    [TestMethod]
    public void GetHomesWhereIMember_WhenUserHasHomes_ShouldReturnListOfShowHomeDto()
    {
        User user = _validCurrentUser;
        var args = new CreateHomeArgs(AddressStreet, AddressNumber, Latitude, Longitude, MaxMembers, HomeName, user);
        var home = new Home(args);
        var expected = new List<ShowHomeDto> { new(home.Id, home.Name) };

        _homeMemberRepositoryMock.Setup(hms => hms.GetAll(h => h.UserId == user.Id))
            .Returns([new HomeMember(home, user)]);

        List<ShowHomeDto> result = _service.GetHomesWhereIMember(user);

        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetHomesWhereIMember_WhenUserHasNoHomes_ShouldReturnEmptyList()
    {
        User user = _validCurrentUser;

        _homeMemberRepositoryMock.Setup(hms => hms.GetAll(h => h.UserId == user.Id)).Returns([]);

        List<ShowHomeDto> result = _service.GetHomesWhereIMember(user);

        result.Should().BeEmpty();
    }

    #endregion

    #region GetHomePermissions

    #region Error

    [TestMethod]
    public void GetHomePermissions_WhenRepositoryThrowsException_ShouldThrowException()
    {
        _homePermissionRepositoryMock.Setup(hpr => hpr.GetAll(null)).Throws(new Exception("Repository error"));

        Action act = () => _service.GetHomePermissions();

        act.Should().Throw<Exception>().WithMessage("Repository error");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetHomePermissions_WhenCalled_ShouldReturnListOfShowHomePermissionDto()
    {
        var permissions = new List<HomePermission>
        {
            new() { Id = Guid.NewGuid(), Name = "Permission1" }, new() { Id = Guid.NewGuid(), Name = "Permission2" }
        };
        var expected = permissions.Select(p => new ShowHomePermissionDto(p.Id, p.Name)).ToList();

        _homePermissionRepositoryMock.Setup(hpr => hpr.GetAll(null)).Returns(permissions);

        List<ShowHomePermissionDto> result = _service.GetHomePermissions();

        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetHomePermissions_WhenNoPermissionsExist_ShouldReturnEmptyList()
    {
        var permissions = new List<HomePermission>();

        _homePermissionRepositoryMock.Setup(hpr => hpr.GetAll(null)).Returns(permissions);

        List<ShowHomePermissionDto> result = _service.GetHomePermissions();

        result.Should().BeEmpty();
    }

    #endregion

    #endregion
}
