using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Tests.Repositories;

[TestClass]
public class SmartDeviceRepositoryTest
{
    private const int Offset = 0;
    private const int Limit = 10;
    private readonly DbContext _context = DbContextBuilder.BuildSmartDeviceRepositoryTestDbContext();
    private readonly SmartDeviceRepository _smartDeviceRepository;
    private SmartDevice _smartDevice = null!;

    internal sealed class TestDbContext(DbContextOptions options)
        : DbContext(options)
    {
        public DbSet<SmartDevice> SmartDevicesTest { get; set; }
    }

    #region Initialize And Finalize

    public SmartDeviceRepositoryTest()
    {
        _smartDeviceRepository = new SmartDeviceRepository(_context);
    }

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();

        var user = new User
        {
            Name = "John",
            LastName = "Doe",
            Email = "Jhon@gmail.com",
            Password = "Password-1",
            Role = new Role("CompanyOwner") { Permissions = [new SystemPermission("Permission1")] },
            CreatedAt = DateTime.Now,
            HasCompany = false
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

        _smartDevice = new SmartDevice
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

        Func<List<SmartDevice>> act = () => _smartDeviceRepository.GetAll(null, Offset, Limit);

        act.Should().Throw<DataAccessException>().WithMessage("Error getting devices from the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetAll_WhenExistOnlyOne_ShouldReturnOne()
    {
        _smartDeviceRepository.Add(_smartDevice);
        _context.SaveChanges();

        List<SmartDevice> smartDevicesSaved = _smartDeviceRepository.GetAll(null, Offset, Limit);

        smartDevicesSaved.Count.Should().Be(1);

        SmartDevice deviceSaved = smartDevicesSaved[0];
        deviceSaved.Should().NotBeNull();
        deviceSaved.Id.Should().Be(_smartDevice.Id);
        deviceSaved.Name.Should().Be(_smartDevice.Name);
        deviceSaved.Model.Should().Be(_smartDevice.Model);
        deviceSaved.Description.Should().Be(_smartDevice.Description);
        deviceSaved.CreateOn.Should().Be(_smartDevice.CreateOn);
        deviceSaved.CompanyOwner.Should().Be(_smartDevice.CompanyOwner);
        deviceSaved.Images.Should().BeEquivalentTo(_smartDevice.Images);
    }

    [TestMethod]
    public void GetAll_WhenExistMultiple_ShouldReturnAll()
    {
        var user = new User
        {
            Name = "Peter",
            LastName = "Doe",
            Email = "Peter@gmail.com",
            Password = "Password-1",
            Role = new Role("CompanyOwner") { Permissions = [new SystemPermission("Permission1")] },
            CreatedAt = DateTime.Now,
            HasCompany = false
        };
        var company = new Company { Name = "IBM", Owner = user, Rut = "1234567890-9", Logo = "logo" };
        var image = new DeviceImage("url.jpg", true);

        var expectedDevice2 = new SmartDevice
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789013"),
            Name = "Go pro",
            Model = "AAA111",
            Description = "Description",
            CreateOn = DateTimeProvider.Now,
            CompanyOwner = company,
            Images = [image],
            DeviceType = DeviceTypeEnum.Camera
        };

        _smartDeviceRepository.Add(_smartDevice);
        _smartDeviceRepository.Add(expectedDevice2);
        _context.SaveChanges();

        List<SmartDevice> smartDevicesSaved = _smartDeviceRepository.GetAll(null, Offset, Limit);

        smartDevicesSaved.Count.Should().Be(2);

        SmartDevice deviceSaved1 = smartDevicesSaved[0];
        deviceSaved1.Id.Should().Be(_smartDevice.Id);
        deviceSaved1.Name.Should().Be(_smartDevice.Name);

        SmartDevice deviceSaved2 = smartDevicesSaved[1];
        deviceSaved2.Id.Should().Be(expectedDevice2.Id);
        deviceSaved2.Name.Should().Be(expectedDevice2.Name);
    }

    [TestMethod]
    public void GetAll_WithPredicate_ReturnsFilter()
    {
        var user = new User
        {
            Name = "Peter",
            LastName = "Doe",
            Email = "Peter@gmail.com",
            Password = "Password-1",
            Role = new Role("CompanyOwner") { Permissions = [new SystemPermission("Permission1")] },
            CreatedAt = DateTime.Now,
            HasCompany = false
        };
        var company = new Company { Name = "IBM", Owner = user, Rut = "1234567890-9", Logo = "logo" };
        var image = new DeviceImage("url.jpg", true);

        var expectedDevice2 = new SmartDevice
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789013"),
            Name = "Go pro",
            Model = "AAA111",
            Description = "Description",
            CreateOn = DateTimeProvider.Now,
            CompanyOwner = company,
            Images = [image],
            DeviceType = DeviceTypeEnum.Camera
        };

        _smartDeviceRepository.Add(_smartDevice);
        _smartDeviceRepository.Add(expectedDevice2);
        _context.SaveChanges();

        List<SmartDevice> smartDevicesSaved = _smartDeviceRepository.GetAll(d => d.Name == "Device", Offset, Limit);

        smartDevicesSaved.Count.Should().Be(1);

        SmartDevice deviceSaved = smartDevicesSaved[0];
        deviceSaved.Id.Should().Be(_smartDevice.Id);
        deviceSaved.Name.Should().Be(_smartDevice.Name);
    }

    [TestMethod]
    public void GetAll_WhenNoEntity_ShouldReturnEmptyList()
    {
        List<SmartDevice> deviceSaved = _smartDeviceRepository.GetAll(null, Offset, Limit);

        deviceSaved.Count.Should().Be(0);
    }

    #region Pagination

    [TestMethod]
    public void GetAll_WhenOffsetAndLimitAreOne_ShouldReturnOnlySecond()
    {
        var user = new User
        {
            Name = "Peter",
            LastName = "Doe",
            Email = "Peter@gmail.com",
            Password = "Password-1",
            Role = new Role("CompanyOwner") { Permissions = [new SystemPermission("Permission1")] },
            CreatedAt = DateTime.Now,
            HasCompany = false
        };
        var company = new Company { Name = "IBM", Owner = user, Rut = "1234567890-9", Logo = "logo" };
        var image = new DeviceImage("url.jpg", true);

        var expectedDevice2 = new SmartDevice
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789013"),
            Name = "Go pro",
            Model = "AAA111",
            Description = "Description",
            CreateOn = DateTimeProvider.Now,
            CompanyOwner = company,
            Images = [image],
            DeviceType = DeviceTypeEnum.Camera
        };

        _smartDeviceRepository.Add(_smartDevice);
        _smartDeviceRepository.Add(expectedDevice2);
        _context.SaveChanges();

        List<SmartDevice> smartDevicesSaved = _smartDeviceRepository.GetAll(null, 1, 1);

        smartDevicesSaved.Count.Should().Be(1);

        SmartDevice deviceSaved = smartDevicesSaved[0];
        deviceSaved.Id.Should().Be(expectedDevice2.Id);
        deviceSaved.Name.Should().Be(expectedDevice2.Name);
    }

    #endregion

    #endregion

    #endregion
}
