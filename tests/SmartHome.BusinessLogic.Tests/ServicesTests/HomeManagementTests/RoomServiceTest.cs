using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.BusinessLogic.Services.HomeManagement;

namespace SmartHome.BusinessLogic.Tests.ServicesTests.HomeManagementTests;

[TestClass]
public class RoomServiceTest
{
    private const string RoomName = "roomName";
    private Mock<IRepository<HomeDevice>> _homeDeviceRepositoryMock = null!;
    private Mock<IRepository<HomeMember>> _homeMemberRepositoryMock = null!;
    private Mock<IRepository<Home>> _homeRepositoryMock = null!;
    private Mock<IRepository<Room>> _roomRepositoryMock = null!;

    private RoomService _service = null!;
    private User _validCurrentUser = null!;
    private Home _validHome = null!;

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
        _homeRepositoryMock = new Mock<IRepository<Home>>(MockBehavior.Strict);
        _homeMemberRepositoryMock = new Mock<IRepository<HomeMember>>(MockBehavior.Strict);
        _homeDeviceRepositoryMock = new Mock<IRepository<HomeDevice>>(MockBehavior.Strict);
        _roomRepositoryMock = new Mock<IRepository<Room>>(MockBehavior.Strict);

        _service = new RoomService(_homeRepositoryMock.Object, _homeDeviceRepositoryMock.Object,
            _homeMemberRepositoryMock.Object, _roomRepositoryMock.Object);

        _validCurrentUser =
            new User(new CreateUserArgs(name, lastName, email, password, profileImage)
            {
                Role = new Role { Id = Constant.HomeOwnerRoleId, Name = "HomeOwner" }
            });

        _validHome = new Home(new CreateHomeArgs(addressStreet, addressNumber, latitude, longitude, maxMembers,
            homeName, _validCurrentUser));
    }

    #region AddAndSave

    #region Error

    [TestMethod]
    public void AddAndSave_WhenHomeIdIsNull_ShouldReturnArgumentNullException()
    {
        Action act = () => _service.AddAndSave(_validCurrentUser, null, RoomName);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'homeId')");
    }

    [TestMethod]
    public void AddAndSave_WhenHomeIdIsEmpty_ShouldReturnArgumentNullException()
    {
        Action act = () => _service.AddAndSave(_validCurrentUser, Guid.Empty, RoomName);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'homeId')");
    }

    [TestMethod]
    public void AddAndSave_WhenNameIsNull_ShouldReturnArgumentNullException()
    {
        Guid? homeId = Guid.NewGuid();
        Action act = () => _service.AddAndSave(_validCurrentUser, homeId, null);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'name')");
    }

    [TestMethod]
    public void AddAndSave_WhenNameIsEmpty_ShouldReturnArgumentNullException()
    {
        Guid? homeId = Guid.NewGuid();
        Action act = () => _service.AddAndSave(_validCurrentUser, homeId, string.Empty);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'name')");
    }

    [TestMethod]
    public void AddAndSave_WhenHomeNotFound_ShouldReturnInvalidOperationException()
    {
        Guid? homeId = Guid.NewGuid();
        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns((Home?)null);

        Action act = () => _service.AddAndSave(_validCurrentUser, homeId, RoomName);

        act.Should().Throw<InvalidOperationException>().WithMessage("Home not found.");
    }

    [TestMethod]
    public void AddAndSave_WhenUserIsNotHomeOwner_ShouldReturnUnauthorizedAccessException()
    {
        Guid? homeId = Guid.NewGuid();
        var user = new User
        {
            Name = "Ana",
            LastName = "Parker",
            Email = "ana@gmail.com",
            Password = "Password-123",
            Role = new Role("HomeOwner")
        };

        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(_validHome);

        Action act = () => _service.AddAndSave(user, homeId, RoomName);

        act.Should().Throw<UnauthorizedAccessException>().WithMessage("User does not is owner of the home.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void AddAndSave_WhenParametersAreValid_ShouldAddAndSave()
    {
        Guid? homeId = Guid.NewGuid();
        User user = _validCurrentUser;
        Home home = _validHome;

        _homeRepositoryMock.Setup(hs => hs.Get(h => h.Id == homeId)).Returns(home);
        _roomRepositoryMock.Setup(rs => rs.Add(It.IsAny<Room>())).Verifiable();

        _service.AddAndSave(user, homeId, RoomName);

        _roomRepositoryMock.Verify(rs => rs.Add(It.Is<Room>(r => r.Name == RoomName)), Times.Once);
    }

    #endregion

    #endregion

    #region AddDeviceAndSave

    #region Error

    [TestMethod]
    public void AddDeviceAndSave_WhenRoomIdIsNull_ShouldReturnArgumentNullException()
    {
        Action act = () => _service.AddDeviceAndSave(_validCurrentUser, null, Guid.NewGuid());

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'roomId')");
    }

    [TestMethod]
    public void AddDeviceAndSave_WhenRoomIdIsEmpty_ShouldReturnArgumentNullException()
    {
        Action act = () => _service.AddDeviceAndSave(_validCurrentUser, Guid.Empty, Guid.NewGuid());

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'roomId')");
    }

    [TestMethod]
    public void AddDeviceAndSave_WhenHardwareIdIsNull_ShouldReturnArgumentNullException()
    {
        Action act = () => _service.AddDeviceAndSave(_validCurrentUser, Guid.NewGuid(), null);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void AddDeviceAndSave_WhenHardwareIdIsEmpty_ShouldReturnArgumentNullException()
    {
        Action act = () => _service.AddDeviceAndSave(_validCurrentUser, Guid.NewGuid(), Guid.Empty);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'hardwareId')");
    }

    [TestMethod]
    public void AddDeviceAndSave_WhenRoomNotFound_ShouldReturnInvalidOperationException()
    {
        Guid? roomId = Guid.NewGuid();
        Guid? hardwareId = Guid.NewGuid();

        _roomRepositoryMock.Setup(rs => rs.Get(r => r.Id == roomId)).Returns((Room?)null);

        Action act = () => _service.AddDeviceAndSave(_validCurrentUser, roomId, hardwareId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Room not found.");
    }

    [TestMethod]
    public void AddDeviceAndSave_WhenHomeDeviceNotFound_ShouldReturnInvalidOperationException()
    {
        Guid? roomId = Guid.NewGuid();
        Guid? hardwareId = Guid.NewGuid();

        _roomRepositoryMock.Setup(rs => rs.Get(r => r.Id == roomId)).Returns(new Room());
        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns((HomeDevice?)null);

        Action act = () => _service.AddDeviceAndSave(_validCurrentUser, roomId, hardwareId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device is not added to home.");
    }

    [TestMethod]
    public void AddDeviceAndSave_WhenNotHasSameHome_ShouldReturnInvalidOperationException()
    {
        var home = new Home { Id = Guid.NewGuid() };
        var room = new Room(RoomName, home);
        Guid? roomId = Guid.NewGuid();
        Guid? hardwareId = Guid.NewGuid();
        var homeDevice = new HomeDevice(new Home(), new SmartDevice());

        _roomRepositoryMock.Setup(rs => rs.Get(r => r.Id == roomId)).Returns(room);
        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.AddDeviceAndSave(_validCurrentUser, roomId, hardwareId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device is not in the same home as the room.");
    }

    [TestMethod]
    public void AddDeviceAndSave_WhenAlreadyAddedToRoom_ShouldReturnInvalidOperationException()
    {
        var room = new Room(RoomName, _validHome);
        Guid? roomId = Guid.NewGuid();
        Guid? hardwareId = Guid.NewGuid();
        var homeDevice = new HomeDevice(_validHome, new SmartDevice()) { Room = room };

        _roomRepositoryMock.Setup(rs => rs.Get(r => r.Id == roomId)).Returns(room);
        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);

        Action act = () => _service.AddDeviceAndSave(_validCurrentUser, roomId, hardwareId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Device is already added to the room.");
    }

    [TestMethod]
    public void AddDeviceAndSave_WhenUserIsNotMember_ShouldReturnUnauthorizedAccessException()
    {
        var user = new User { Id = Guid.NewGuid() };
        var room = new Room(RoomName, _validHome);
        Guid? roomId = Guid.NewGuid();
        Guid? hardwareId = Guid.NewGuid();
        var homeDevice = new HomeDevice(_validHome, new SmartDevice());

        _roomRepositoryMock.Setup(rs => rs.Get(r => r.Id == roomId)).Returns(room);
        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);
        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.HomeId == room.Home.Id && hm.UserId == user.Id))
            .Returns((HomeMember?)null);

        Action act = () => _service.AddDeviceAndSave(user, roomId, hardwareId);

        act.Should().Throw<UnauthorizedAccessException>().WithMessage("User does not is member of the home.");
    }

    [TestMethod]
    public void AddDeviceAndSave_WhenUserIsMemberWithoutPermissions_ShouldReturnUnauthorizedAccessException()
    {
        var user = new User { Id = Guid.NewGuid() };
        var room = new Room(RoomName, _validHome);
        Guid? roomId = Guid.NewGuid();
        Guid? hardwareId = Guid.NewGuid();
        var homeDevice = new HomeDevice(_validHome, new SmartDevice());
        var homeMember = new HomeMember(_validHome, user);

        _roomRepositoryMock.Setup(rs => rs.Get(r => r.Id == roomId)).Returns(room);
        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);
        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.HomeId == room.Home.Id && hm.UserId == user.Id))
            .Returns(homeMember);

        Action act = () => _service.AddDeviceAndSave(user, roomId, hardwareId);

        act.Should().Throw<UnauthorizedAccessException>()
            .WithMessage("User does not have permission to add device to room.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void AddDeviceAndSave_WhenCurrentIsOwner_ShouldAddDeviceAndSave()
    {
        User user = _validCurrentUser;
        var room = new Room(RoomName, _validHome);
        Guid? roomId = Guid.NewGuid();
        Guid? hardwareId = Guid.NewGuid();
        var homeDevice = new HomeDevice(_validHome, new SmartDevice());
        var homeMember = new HomeMember(_validHome, user);

        _roomRepositoryMock.Setup(rs => rs.Get(r => r.Id == roomId)).Returns(room);
        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);
        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.HomeId == room.Home.Id && hm.UserId == user.Id))
            .Returns(homeMember);
        _homeDeviceRepositoryMock.Setup(hds => hds.Update(It.IsAny<HomeDevice>())).Verifiable();

        _service.AddDeviceAndSave(user, roomId, hardwareId);

        homeDevice.Room.Should().Be(room);
        _homeDeviceRepositoryMock.Verify(hds => hds.Update(It.Is<HomeDevice>(hd => hd.Room == room)), Times.Once);
    }

    [TestMethod]
    public void AddDeviceAndSave_WhenCurrentIsMemberWithPermissions_ShouldAddDeviceAndSave()
    {
        var user = new User { Id = Guid.NewGuid() };
        var room = new Room(RoomName, _validHome);
        Guid? roomId = Guid.NewGuid();
        Guid? hardwareId = Guid.NewGuid();
        var homeDevice = new HomeDevice(_validHome, new SmartDevice());
        List<HomePermission> homePermissions =
            [new HomePermission { Id = Constant.AddDeviceToRoomId, Name = "AddDeviceAndSave" }];
        var homeMember = new HomeMember(_validHome, user) { Permissions = homePermissions };

        _roomRepositoryMock.Setup(rs => rs.Get(r => r.Id == roomId)).Returns(room);
        _homeDeviceRepositoryMock.Setup(hds => hds.Get(hd => hd.Id == hardwareId)).Returns(homeDevice);
        _homeMemberRepositoryMock.Setup(hms => hms.Get(hm => hm.HomeId == room.Home.Id && hm.UserId == user.Id))
            .Returns(homeMember);
        _homeDeviceRepositoryMock.Setup(hds => hds.Update(It.IsAny<HomeDevice>())).Verifiable();

        _service.AddDeviceAndSave(user, roomId, hardwareId);

        homeDevice.Room.Should().Be(room);
        _homeDeviceRepositoryMock.Verify(hds => hds.Update(It.Is<HomeDevice>(hd => hd.Room == room)), Times.Once);
    }

    #endregion

    #endregion

    #region GetRooms

    #region Error

    [TestMethod]
    public void GetRooms_WhenHomeIdIsNull_ShouldReturnArgumentNullException()
    {
        Action act = () => _service.GetRooms(null, _validCurrentUser);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'homeId')");
    }

    [TestMethod]
    public void GetRooms_WhenHomeIdIsEmpty_ShouldReturnArgumentNullException()
    {
        Action act = () => _service.GetRooms(Guid.Empty, _validCurrentUser);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'homeId')");
    }

    [TestMethod]
    public void GetRooms_WhenHomeMemberNotFound_ShouldReturnInvalidOperationException()
    {
        Guid? homeId = Guid.NewGuid();
        User user = _validCurrentUser;

        _homeMemberRepositoryMock.Setup(hs => hs.Get(hm => hm.HomeId == homeId && hm.UserId == user.Id))
            .Returns((HomeMember?)null);

        Action act = () => _service.GetRooms(homeId, user);

        act.Should().Throw<UnauthorizedAccessException>().WithMessage("User does not is member of the home.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetRooms_WhenParametersAreValid_ShouldReturnRooms()
    {
        Guid? homeId = Guid.NewGuid();
        User currentUser = _validCurrentUser;
        var rooms = new List<Room> { new("Living Room", _validHome), new("Bedroom", _validHome) };
        var homeMember = new HomeMember(_validHome, currentUser);

        _homeMemberRepositoryMock.Setup(hs => hs.Get(hm => hm.HomeId == homeId && hm.UserId == currentUser.Id))
            .Returns(homeMember);
        _roomRepositoryMock.Setup(rs => rs.GetAll(r => r.Home.Id == homeId)).Returns(rooms);

        List<ShowRoomDto> result = _service.GetRooms(homeId, currentUser);

        result.Should().HaveCount(2);
        result.Should().Contain(r => r.Name == "Living Room");
        result.Should().Contain(r => r.Name == "Bedroom");
    }

    #endregion

    #endregion
}
