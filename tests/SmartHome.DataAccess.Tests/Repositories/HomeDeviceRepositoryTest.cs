using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Tests.Repositories;

[TestClass]
public class HomeDeviceRepositoryTest
{
    private readonly DbContext _context = DbContextBuilder.BuildHomeDeviceRepositoryTestDbContext();
    private readonly HomeDeviceRepository _homeDeviceDeviceRepository;
    private HomeDevice _homeDevice = null!;

    internal sealed class TestDbContext(DbContextOptions options)
        : DbContext(options)
    {
        public DbSet<HomeDevice> HomeDeviceTest { get; set; }
    }

    #region Initialize And Finalize

    public HomeDeviceRepositoryTest()
    {
        _homeDeviceDeviceRepository = new HomeDeviceRepository(_context);
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
            Owner = user,
            Name = "home"
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
        _homeDevice = new HomeDevice
        {
            Id = Guid.Parse("12345679-1234-1234-1234-123456711112"),
            Home = home,
            Device = smartDevice,
            IsConnected = true
        };
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #endregion

    #region Get

    #region Error

    [TestMethod]
    public void Get_WhenError_ShouldThrowDataAccessException()
    {
        _context.Database.EnsureDeleted();

        Func<HomeDevice?> act = () => _homeDeviceDeviceRepository.Get(e => e.Id == Guid.NewGuid());

        act.Should().Throw<DataAccessException>().WithMessage("Error getting home device from the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Get_WhenNotExists_ReturnsNull()
    {
        HomeDevice? homeDevice = _homeDeviceDeviceRepository.Get(e => e.Id == _homeDevice.Id);

        homeDevice.Should().BeNull();
    }

    [TestMethod]
    public void Get_WhenExists_ShouldReturnUser()
    {
        _homeDeviceDeviceRepository.Add(_homeDevice);
        _context.SaveChanges();

        HomeDevice? homeDeviceSaved = _homeDeviceDeviceRepository.Get(u => u.Id == _homeDevice.Id);

        homeDeviceSaved.Should().NotBeNull();
        homeDeviceSaved.Id.Should().Be(_homeDevice.Id);
        homeDeviceSaved.Home.Should().Be(_homeDevice.Home);
        homeDeviceSaved.Device.Should().Be(_homeDevice.Device);
        homeDeviceSaved.IsConnected.Should().Be(_homeDevice.IsConnected);
    }

    #endregion

    #endregion
}
