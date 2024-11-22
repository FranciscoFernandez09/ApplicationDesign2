using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Tests.Repositories;

[TestClass]
public class NotificationRepositoryTest
{
    private readonly DbContext _context = DbContextBuilder.BuildNotificationRepositoryTestDbContext();
    private readonly NotificationRepository _notificationRepository;
    private Notification _notification = null!;
    private Notification _notification2 = null!;

    internal sealed class TestDbContext(DbContextOptions options)
        : DbContext(options)
    {
        public DbSet<Notification> NotificationTest { get; set; }
    }

    #region Initialize And Finalize

    public NotificationRepositoryTest()
    {
        _notificationRepository = new NotificationRepository(_context);
    }

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        var user = new User
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789012"),
            Name = "John",
            LastName = "Doe",
            Email = "Jhon@gmail.com",
            Password = "Password-1",
            Role = new Role("HomeOwner") { Permissions = [new SystemPermission("Permission1")] },
            CreatedAt = DateTime.Now,
            HasCompany = false
        };
        var home = new Home
        {
            Id = Guid.Parse("12345679-1234-1234-1234-123456789012"),
            AddressStreet = "Street",
            AddressNumber = 123,
            Latitude = 10,
            Longitude = 10,
            MaxMembers = 5,
            Name = "name",
            Owner = user
        };
        var company = new Company
        {
            Id = Guid.Parse("12345679-1234-1234-1234-123456789012"),
            Name = "Company",
            Owner = user,
            Rut = "1234567890-9",
            Logo = "logo"
        };
        var image = new DeviceImage("url.png", true);

        var smartDevice = new SmartDevice
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789012"),
            Name = "Device",
            Model = "AAA111",
            Description = "Description",
            CreateOn = DateTimeProvider.Now,
            CompanyOwner = company,
            Images = [image],
            DeviceType = DeviceTypeEnum.Camera
        };
        var homeDevice = new HomeDevice
        {
            Id = Guid.Parse("12345679-1234-1234-1234-123456711112"),
            Home = home,
            Device = smartDevice,
            IsConnected = true
        };
        var homeMember = new HomeMember(home, user);
        _notification = new Notification
        {
            Id = Guid.Parse("12345679-1234-1234-1234-123456789012"),
            EventDate = DateTimeProvider.Now,
            Event = "Event",
            IsRead = false,
            HomeDevice = homeDevice,
            Members = [homeMember]
        };

        _notification2 = new Notification
        {
            Id = Guid.Parse("12345679-1234-1234-1234-123456789999"),
            EventDate = DateTimeProvider.Now,
            Event = "Event2",
            IsRead = false,
            HomeDevice = homeDevice,
            Members = [homeMember]
        };
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #endregion

    #region GetAll

    #region Error

    [TestMethod]
    public void GetAll_WhenError_ShouldThrowDataAccessException()
    {
        _context.Database.EnsureDeleted();

        Func<List<Notification>> act = () => _notificationRepository.GetAll();

        act.Should().Throw<DataAccessException>().WithMessage("Error getting notifications from the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetAll_WhenExistOnlyOne_ShouldReturnOne()
    {
        _notificationRepository.Add(_notification);
        _context.SaveChanges();

        List<Notification> notificationsSaved = _notificationRepository.GetAll();

        notificationsSaved.Count.Should().Be(1);

        Notification notificationSaved = notificationsSaved[0];
        notificationSaved.Should().NotBeNull();
        notificationSaved.Id.Should().Be(_notification.Id);
        notificationSaved.EventDate.Should().Be(_notification.EventDate);
        notificationSaved.Event.Should().Be(_notification.Event);
        notificationSaved.IsRead.Should().Be(_notification.IsRead);
        notificationSaved.HomeDevice.Should().Be(_notification.HomeDevice);
        notificationSaved.Members.Should().BeEquivalentTo(_notification.Members);
    }

    [TestMethod]
    public void GetAll_WhenExistMultiple_ShouldReturnAll()
    {
        _notificationRepository.Add(_notification);
        _notificationRepository.Add(_notification2);
        _context.SaveChanges();

        List<Notification> notificationsSaved = _notificationRepository.GetAll();

        notificationsSaved.Count.Should().Be(2);

        Notification notificationSaved = notificationsSaved[0];
        notificationSaved.Id.Should().Be(_notification.Id);
        notificationSaved.EventDate.Should().Be(_notification.EventDate);
        notificationSaved.Event.Should().Be(_notification.Event);
        notificationSaved.IsRead.Should().Be(_notification.IsRead);
        notificationSaved.HomeDevice.Should().Be(_notification.HomeDevice);
        notificationSaved.Members.Should().BeEquivalentTo(_notification.Members);

        notificationSaved = notificationsSaved[1];
        notificationSaved.Id.Should().Be(_notification2.Id);
        notificationSaved.EventDate.Should().Be(_notification2.EventDate);
        notificationSaved.Event.Should().Be(_notification2.Event);
        notificationSaved.IsRead.Should().Be(_notification2.IsRead);
        notificationSaved.HomeDevice.Should().Be(_notification2.HomeDevice);
        notificationSaved.Members.Should().BeEquivalentTo(_notification2.Members);
    }

    [TestMethod]
    public void GetAll_WithPredicate_ReturnsFilter()
    {
        _notificationRepository.Add(_notification);
        _notificationRepository.Add(_notification2);
        _context.SaveChanges();

        List<Notification> notificationsSaved = _notificationRepository.GetAll(n => n.Event == "Event2");

        notificationsSaved.Count.Should().Be(1);

        Notification notificationSaved = notificationsSaved[0];
        notificationSaved.Id.Should().Be(_notification2.Id);
        notificationSaved.EventDate.Should().Be(_notification2.EventDate);
        notificationSaved.Event.Should().Be(_notification2.Event);
        notificationSaved.IsRead.Should().Be(_notification2.IsRead);
        notificationSaved.HomeDevice.Should().Be(_notification2.HomeDevice);
        notificationSaved.Members.Should().BeEquivalentTo(_notification2.Members);
    }

    [TestMethod]
    public void GetAll_WhenNoEntity_ShouldReturnEmptyList()
    {
        List<Notification> notificationsSaved = _notificationRepository.GetAll();

        notificationsSaved.Count.Should().Be(0);
    }

    #endregion

    #endregion
}
