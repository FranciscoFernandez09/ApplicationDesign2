using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Tests.Repositories;

[TestClass]
public class UserRepositoryTest
{
    private const int Offset = 0;
    private const int Limit = 10;
    private readonly DbContext _context = DbContextBuilder.BuildUserRepositoryTestDbContext();
    private readonly UserRepository _userRepository;
    private User _user = null!;

    internal sealed class TestDbContext(DbContextOptions options)
        : DbContext(options)
    {
        public DbSet<User> UserTest { get; set; }
    }

    #region Initialize And Finalize

    public UserRepositoryTest()
    {
        _userRepository = new UserRepository(_context);
    }

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _user = new User
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789012"),
            Name = "John",
            LastName = "Doe",
            Email = "Jhon@gmail.com",
            Password = "Password-1",
            Role = new Role("Admin") { Permissions = [new SystemPermission("Permission1")] },
            CreatedAt = DateTime.Now,
            HasCompany = false
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

        Func<User?> act = () => _userRepository.Get(e => e.Id == Guid.NewGuid());

        act.Should().Throw<DataAccessException>().WithMessage("Error getting user from the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Get_WhenNotExists_ReturnsNull()
    {
        var expectedUser = new User();

        User? user = _userRepository.Get(e => e.Id == expectedUser.Id);

        user.Should().BeNull();
    }

    [TestMethod]
    public void Get_WhenExists_ShouldReturnUser()
    {
        var expectedUser = new User
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123457779012"),
            Name = "John",
            LastName = "Doe",
            Email = "Jhon@gmail.com",
            Password = "Password-1",
            Role = new Role("Admin") { Permissions = [new SystemPermission("Permission1")] },
            CreatedAt = DateTime.Now,
            HasCompany = false
        };

        _userRepository.Add(expectedUser);
        _context.SaveChanges();

        User? userSaved = _userRepository.Get(u => u.Id == expectedUser.Id);

        userSaved.Should().NotBeNull();
        userSaved.Id.Should().Be(expectedUser.Id);
        userSaved.Name.Should().Be(expectedUser.Name);
        userSaved.LastName.Should().Be(expectedUser.LastName);
        userSaved.Email.Should().Be(expectedUser.Email);
        userSaved.Password.Should().Be(expectedUser.Password);
        userSaved.Role.Should().Be(expectedUser.Role);
        userSaved.Role.Permissions.Should().BeEquivalentTo(expectedUser.Role.Permissions);
        userSaved.CreatedAt.Should().Be(expectedUser.CreatedAt);
        userSaved.HasCompany.Should().Be(expectedUser.HasCompany);
    }

    #endregion

    #endregion

    #region GetAll

    #region Error

    [TestMethod]
    public void GetAll_WhenError_ShouldThrowDataAccessException()
    {
        _context.Database.EnsureDeleted();

        Func<List<User>> act = () => _userRepository.GetAll(null, Offset, Limit);

        act.Should().Throw<DataAccessException>().WithMessage("Error getting users from the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetAll_WhenExistOnlyOne_ShouldReturnOne()
    {
        _userRepository.Add(_user);
        _context.SaveChanges();

        List<User> usersSaved = _userRepository.GetAll(null, Offset, Limit);

        usersSaved.Count.Should().Be(1);

        User userSaved = usersSaved[0];
        userSaved.Should().NotBeNull();
        userSaved.Id.Should().Be(_user.Id);
        userSaved.Name.Should().Be(_user.Name);
        userSaved.LastName.Should().Be(_user.LastName);
        userSaved.Email.Should().Be(_user.Email);
        userSaved.Password.Should().Be(_user.Password);
        userSaved.Role.Should().Be(_user.Role);
        userSaved.Role.Permissions.Should().BeEquivalentTo(_user.Role.Permissions);
        userSaved.CreatedAt.Should().Be(_user.CreatedAt);
        userSaved.HasCompany.Should().Be(_user.HasCompany);
    }

    [TestMethod]
    public void GetAll_WhenExistMultiple_ShouldReturnAll()
    {
        var expectedUser2 = new User
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789013"),
            Name = "Peter",
            LastName = "Parker",
            Email = "Peter@gmail.com",
            Password = "Password-1",
            Role = new Role("HomeOwner"),
            CreatedAt = DateTime.Now,
            HasCompany = true
        };

        _userRepository.Add(_user);
        _userRepository.Add(expectedUser2);
        _context.SaveChanges();

        List<User> usersSaved = _userRepository.GetAll(null, Offset, Limit);

        usersSaved.Count.Should().Be(2);

        User userSaved1 = usersSaved[0];
        userSaved1.Id.Should().Be(_user.Id);
        userSaved1.Name.Should().Be(_user.Name);

        User userSaved2 = usersSaved[1];
        userSaved2.Id.Should().Be(expectedUser2.Id);
        userSaved2.Name.Should().Be(expectedUser2.Name);
    }

    [TestMethod]
    public void GetAll_WithPredicate_ReturnsFilter()
    {
        var expectedUser2 = new User
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789013"),
            Name = "Peter",
            LastName = "Parker",
            Email = "Peter@gmail.com",
            Password = "Password-1",
            Role = new Role("HomeOwner"),
            CreatedAt = DateTime.Now,
            HasCompany = true
        };

        _userRepository.Add(_user);
        _userRepository.Add(expectedUser2);
        _context.SaveChanges();

        List<User> usersSaved = _userRepository.GetAll(e => e.Name == "John", Offset, Limit);

        usersSaved.Count.Should().Be(1);

        User userSaved = usersSaved[0];
        userSaved.Id.Should().Be(_user.Id);
        userSaved.Name.Should().Be(_user.Name);
    }

    [TestMethod]
    public void GetAll_WhenNoEntity_ShouldReturnEmptyList()
    {
        List<User> usersSaved = _userRepository.GetAll(null, Offset, Limit);

        usersSaved.Count.Should().Be(0);
    }

    #region Pagination

    [TestMethod]
    public void GetAll_WhenOffsetAndLimitAreOne_ShouldReturnOnlySecond()
    {
        var expectedUser2 = new User
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789013"),
            Name = "Peter",
            LastName = "Parker",
            Email = "Peter@gmail.com",
            Password = "Password-1",
            Role = new Role("HomeOwner"),
            CreatedAt = DateTime.Now,
            HasCompany = true
        };

        _userRepository.Add(_user);
        _userRepository.Add(expectedUser2);
        _context.SaveChanges();

        List<User> usersSaved = _userRepository.GetAll(null, 1, 1);

        usersSaved.Count.Should().Be(1);

        User userSaved = usersSaved[0];
        userSaved.Id.Should().Be(expectedUser2.Id);
        userSaved.Name.Should().Be(expectedUser2.Name);
    }

    #endregion

    #endregion

    #endregion
}
