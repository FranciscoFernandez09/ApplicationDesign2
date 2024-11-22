using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Tests.Repositories;

[TestClass]
public class HomeRepositoryTest
{
    private readonly DbContext _context = DbContextBuilder.BuildHomeRepositoryTestDbContext();
    private readonly HomeRepository _homeRepository;
    private Home _home = null!;

    internal sealed class TestDbContext(DbContextOptions options)
        : DbContext(options)
    {
        public DbSet<Home> HomeTest { get; set; }
    }

    #region Initialize And Finalize

    public HomeRepositoryTest()
    {
        _homeRepository = new HomeRepository(_context);
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
        _home = new Home
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
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #endregion

    #region Add

    #region Error

    [TestMethod]
    public void Add_WhenError_ShouldThrowDataAccessException()
    {
        _context.Database.EnsureDeleted();
        var duplicateHome = new Home
        {
            Id = _home.Id,
            AddressStreet = "Street",
            AddressNumber = 123,
            Latitude = 10,
            Longitude = 10,
            MaxMembers = 5,
            Name = "name",
            Owner = _home.Owner
        };

        _homeRepository.Add(_home);

        Action act = () => _homeRepository.Add(duplicateHome);

        act.Should().Throw<DataAccessException>().WithMessage("Error adding home to the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Add_WhenCalled_ShouldAddedToDataBase()
    {
        _homeRepository.Add(_home);
        _context.SaveChanges();

        using TestDbContext otherContext = DbContextBuilder.BuildHomeRepositoryTestDbContext();

        var homesSaved = otherContext.HomeTest.ToList();

        homesSaved.Count.Should().Be(1);

        Home homeSaved = homesSaved[0];
        homeSaved.Id.Should().Be(_home.Id);
        homeSaved.AddressStreet.Should().Be(_home.AddressStreet);
        homeSaved.AddressNumber.Should().Be(_home.AddressNumber);
        homeSaved.Latitude.Should().Be(_home.Latitude);
        homeSaved.Longitude.Should().Be(_home.Longitude);
        homeSaved.MaxMembers.Should().Be(_home.MaxMembers);
    }

    #endregion

    #endregion

    #region Get

    #region Error

    [TestMethod]
    public void Get_WhenError_ShouldThrowDataAccessException()
    {
        _context.Database.EnsureDeleted();

        Func<Home?> act = () => _homeRepository.Get(e => e.Id == Guid.NewGuid());

        act.Should().Throw<DataAccessException>().WithMessage("Error getting home from the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Get_WhenNotExists_ReturnsNull()
    {
        Home? home = _homeRepository.Get(e => e.Id == _home.Id);

        home.Should().BeNull();
    }

    [TestMethod]
    public void Get_WhenExists_ShouldReturnUser()
    {
        _homeRepository.Add(_home);
        _context.SaveChanges();

        Home? homeSaved = _homeRepository.Get(u => u.Id == _home.Id);

        homeSaved.Should().NotBeNull();
        homeSaved.Id.Should().Be(_home.Id);
        homeSaved.AddressStreet.Should().Be(_home.AddressStreet);
        homeSaved.AddressNumber.Should().Be(_home.AddressNumber);
        homeSaved.Latitude.Should().Be(_home.Latitude);
        homeSaved.Longitude.Should().Be(_home.Longitude);
        homeSaved.MaxMembers.Should().Be(_home.MaxMembers);
        homeSaved.Owner.Should().Be(_home.Owner);
    }

    #endregion

    #endregion

    #region GetAll

    #region Error

    [TestMethod]
    public void GetAll_WhenError_ShouldThrowDataAccessException()
    {
        _context.Database.EnsureDeleted();

        Action act = () => _homeRepository.GetAll(e => e.Id == Guid.NewGuid());

        act.Should().Throw<DataAccessException>().WithMessage("Error getting homes from the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetAll_WhenNoHomesExist_ShouldReturnEmptyList()
    {
        List<Home> homes = _homeRepository.GetAll(e => e.Id == _home.Id);

        homes.Should().BeEmpty();
    }

    [TestMethod]
    public void GetAll_WhenHomesExist_ShouldReturnHomes()
    {
        _homeRepository.Add(_home);
        _context.SaveChanges();

        List<Home> homes = _homeRepository.GetAll(e => e.Id == _home.Id);

        homes.Should().NotBeEmpty();
        homes.Should().HaveCount(1);

        Home homeSaved = homes.First();
        homeSaved.Id.Should().Be(_home.Id);
        homeSaved.AddressStreet.Should().Be(_home.AddressStreet);
        homeSaved.AddressNumber.Should().Be(_home.AddressNumber);
        homeSaved.Latitude.Should().Be(_home.Latitude);
        homeSaved.Longitude.Should().Be(_home.Longitude);
        homeSaved.MaxMembers.Should().Be(_home.MaxMembers);
        homeSaved.Owner.Should().Be(_home.Owner);
    }

    #endregion

    #endregion
}
