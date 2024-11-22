using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Tests.Repositories;

[TestClass]
public class HomeMemberRepositoryTest
{
    private readonly DbContext _context = DbContextBuilder.BuildHomeMemberRepositoryTestDbContext();
    private readonly HomeMemberRepository _homeMemberRepository;
    private HomeMember _homeMember = null!;

    internal sealed class TestDbContext(DbContextOptions options)
        : DbContext(options)
    {
        public DbSet<HomeMember> HomeMembersTest { get; set; }
    }

    #region Initialize And Finalize

    public HomeMemberRepositoryTest()
    {
        _homeMemberRepository = new HomeMemberRepository(_context);
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
            Role = new Role("HomeOwner") { Permissions = [new SystemPermission("Permission1")] },
            CreatedAt = DateTime.Now,
            HasCompany = false
        };
        var home = new Home
        {
            Id = Guid.Parse("12345679-1234-1234-1234-123456789012"),
            Name = "Home",
            AddressStreet = "Street",
            AddressNumber = 123,
            Latitude = 123,
            Longitude = 123,
            MaxMembers = 10,
            Owner = user,
            MembersCount = 1
        };

        _homeMember = new HomeMember
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789012"),
            Home = home,
            User = user,
            ShouldNotify = true
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

        Func<HomeMember?> act = () => _homeMemberRepository.Get(e => e.Id == Guid.NewGuid());

        act.Should().Throw<DataAccessException>().WithMessage("Error getting home member from the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Get_WhenNotExists_ReturnsNull()
    {
        HomeMember? home = _homeMemberRepository.Get(e => e.Id == _homeMember.Id);

        home.Should().BeNull();
    }

    [TestMethod]
    public void Get_WhenExists_ShouldReturnUser()
    {
        _homeMemberRepository.Add(_homeMember);
        _context.SaveChanges();

        HomeMember? homeMemberSaved = _homeMemberRepository.Get(u => u.Id == _homeMember.Id);

        homeMemberSaved.Should().NotBeNull();
        homeMemberSaved.Id.Should().Be(_homeMember.Id);
        homeMemberSaved.Home.Should().Be(_homeMember.Home);
        homeMemberSaved.User.Should().Be(_homeMember.User);
        homeMemberSaved.ShouldNotify.Should().Be(_homeMember.ShouldNotify);
    }

    #endregion

    #endregion

    #region GetAll

    #region Error

    [TestMethod]
    public void GetAll_WhenError_ShouldThrowDataAccessException()
    {
        _context.Database.EnsureDeleted();

        Func<List<HomeMember>> act = () => _homeMemberRepository.GetAll();

        act.Should().Throw<DataAccessException>().WithMessage("Error getting home members from the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetAll_WhenExistOnlyOne_ShouldReturnOne()
    {
        _homeMemberRepository.Add(_homeMember);
        _context.SaveChanges();

        List<HomeMember> homeMembersSaved = _homeMemberRepository.GetAll();

        homeMembersSaved.Count.Should().Be(1);

        HomeMember homeMemberSaved = homeMembersSaved[0];
        homeMemberSaved.Should().NotBeNull();
        homeMemberSaved.Id.Should().Be(_homeMember.Id);
        homeMemberSaved.Home.Should().Be(_homeMember.Home);
        homeMemberSaved.User.Should().Be(_homeMember.User);
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

        var expectedHomeMember = new HomeMember
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789013"),
            Home = _homeMember.Home,
            User = user
        };

        _homeMemberRepository.Add(_homeMember);
        _homeMemberRepository.Add(expectedHomeMember);
        _context.SaveChanges();

        List<HomeMember> homeMembersSaved = _homeMemberRepository.GetAll();

        homeMembersSaved.Count.Should().Be(2);

        HomeMember homeMemberSaved1 = homeMembersSaved[0];
        homeMemberSaved1.Id.Should().Be(_homeMember.Id);
        homeMemberSaved1.Home.Should().Be(_homeMember.Home);
        homeMemberSaved1.User.Should().Be(_homeMember.User);

        HomeMember homeMemberSaved2 = homeMembersSaved[1];
        homeMemberSaved2.Id.Should().Be(expectedHomeMember.Id);
        homeMemberSaved2.Home.Should().Be(expectedHomeMember.Home);
        homeMemberSaved2.User.Should().Be(expectedHomeMember.User);
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

        var expectedHomeMember = new HomeMember
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789013"),
            Home = _homeMember.Home,
            User = user
        };

        _homeMemberRepository.Add(_homeMember);
        _homeMemberRepository.Add(expectedHomeMember);
        _context.SaveChanges();

        List<HomeMember> homeMembersSaved = _homeMemberRepository.GetAll(d => d.User.Name == "Peter");

        homeMembersSaved.Count.Should().Be(1);

        HomeMember homeMemberSaved = homeMembersSaved[0];
        homeMemberSaved.Id.Should().Be(expectedHomeMember.Id);
        homeMemberSaved.Home.Should().Be(expectedHomeMember.Home);
        homeMemberSaved.User.Should().Be(expectedHomeMember.User);
    }

    [TestMethod]
    public void GetAll_WhenNoEntity_ShouldReturnEmptyList()
    {
        List<HomeMember> homeMemberSaved = _homeMemberRepository.GetAll();

        homeMemberSaved.Count.Should().Be(0);
    }

    #endregion

    #endregion
}
