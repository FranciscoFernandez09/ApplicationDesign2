using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Domain;
using SmartHome.DataAccess.Repositories;

namespace SmartHome.DataAccess.Tests.Repositories;

[TestClass]
public class CompanyRepositoryTest
{
    private const int Offset = 0;
    private const int Limit = 10;
    private readonly CompanyRepository _companyRepository;
    private readonly DbContext _context = DbContextBuilder.BuildCompanyRepositoryTestDbContext();
    private Company _company = null!;

    internal sealed class TestDbContext(DbContextOptions options)
        : DbContext(options)
    {
        public DbSet<Company> CompanyTest { get; set; }
    }

    #region Initialize And Finalize

    public CompanyRepositoryTest()
    {
        _companyRepository = new CompanyRepository(_context);
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
            Role = new Role("CompanyOwner") { Permissions = [new SystemPermission("Permission1")] },
            CreatedAt = DateTime.Now,
            HasCompany = false
        };
        _company = new Company
        {
            Id = Guid.Parse("12345679-1234-1234-1234-123456789012"),
            Name = "Company",
            Owner = user,
            Rut = "1234567890-9",
            Logo = "logo"
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
        var duplicateCompany = new Company
        {
            Id = _company.Id,
            Name = "Duplicate Company",
            Owner = _company.Owner,
            Rut = "1234567890-9",
            Logo = "logo"
        };

        _companyRepository.Add(_company);

        Action act = () => _companyRepository.Add(duplicateCompany);

        act.Should().Throw<DataAccessException>().WithMessage("Error adding company to the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Add_WhenCalled_ShouldAddedToDataBase()
    {
        _companyRepository.Add(_company);
        _context.SaveChanges();

        using TestDbContext otherContext = DbContextBuilder.BuildCompanyRepositoryTestDbContext();

        var companiesSaved = otherContext.CompanyTest.ToList();

        companiesSaved.Count.Should().Be(1);

        Company companySaved = companiesSaved[0];
        companySaved.Id.Should().Be(_company.Id);
        companySaved.Name.Should().Be(_company.Name);
    }

    #endregion

    #endregion

    #region GetAll

    #region Error

    [TestMethod]
    public void GetAll_WhenError_ShouldThrowDataAccessException()
    {
        _context.Database.EnsureDeleted();

        Func<List<Company>> act = () => _companyRepository.GetAll(null, Offset, Limit);

        act.Should().Throw<DataAccessException>().WithMessage("Error getting companies from the database.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetAll_WhenExistOnlyOne_ShouldReturnOne()
    {
        _companyRepository.Add(_company);
        _context.SaveChanges();

        List<Company> companiesSaved = _companyRepository.GetAll(null, Offset, Limit);

        companiesSaved.Count.Should().Be(1);

        Company companySaved = companiesSaved[0];
        companySaved.Should().NotBeNull();
        companySaved.Id.Should().Be(_company.Id);
        companySaved.Name.Should().Be(_company.Name);
        companySaved.Owner.Should().Be(_company.Owner);
        companySaved.Owner.Name.Should().Be(_company.Owner.Name);
        companySaved.Owner.Email.Should().Be(_company.Owner.Email);
        companySaved.Owner.Role.Should().Be(_company.Owner.Role);
        companySaved.Owner.Role.Permissions.Should().BeEquivalentTo(_company.Owner.Role.Permissions);
        companySaved.Rut.Should().Be(_company.Rut);
        companySaved.Logo.Should().Be(_company.Logo);
    }

    [TestMethod]
    public void GetAll_WhenExistMultiple_ShouldReturnAll()
    {
        var owner = new User
        {
            Name = "Peter",
            LastName = "Doe",
            Email = "Peter@gmail.com",
            Password = "Password-1",
            Role = new Role("CompanyOwner"),
            CreatedAt = DateTime.Now,
            HasCompany = false
        };
        var expectedCompany2 = new Company
        {
            Id = Guid.Parse("12345679-1234-1234-1234-123456789999"),
            Name = "IBM",
            Owner = owner,
            Rut = "1234567890-9",
            Logo = "logo"
        };

        _companyRepository.Add(_company);
        _companyRepository.Add(expectedCompany2);
        _context.SaveChanges();

        List<Company> companiesSaved = _companyRepository.GetAll(null, Offset, Limit);

        companiesSaved.Count.Should().Be(2);

        Company companySaved1 = companiesSaved[0];
        companySaved1.Id.Should().Be(_company.Id);
        companySaved1.Name.Should().Be(_company.Name);

        Company companySaved2 = companiesSaved[1];
        companySaved2.Id.Should().Be(expectedCompany2.Id);
        companySaved2.Name.Should().Be(expectedCompany2.Name);
    }

    [TestMethod]
    public void GetAll_WithPredicate_ReturnsFilter()
    {
        var owner = new User
        {
            Name = "Peter",
            LastName = "Doe",
            Email = "Peter@gmail.com",
            Password = "Password-1",
            Role = new Role("CompanyOwner"),
            CreatedAt = DateTime.Now,
            HasCompany = false
        };
        var expectedCompany2 = new Company { Name = "IBM", Owner = owner, Rut = "1234567890-9", Logo = "logo" };

        _companyRepository.Add(_company);
        _companyRepository.Add(expectedCompany2);
        _context.SaveChanges();

        List<Company> companiesSaved = _companyRepository.GetAll(c => c.Name == "Company", Offset, Limit);

        companiesSaved.Count.Should().Be(1);

        Company companySaved = companiesSaved[0];
        companySaved.Id.Should().Be(_company.Id);
        companySaved.Name.Should().Be(_company.Name);
    }

    [TestMethod]
    public void GetAll_WhenNoEntity_ShouldReturnEmptyList()
    {
        List<Company> companiesSaved = _companyRepository.GetAll(null, Offset, Limit);

        companiesSaved.Count.Should().Be(0);
    }

    #region Pagination

    [TestMethod]
    public void GetAll_WhenOffsetAndLimitAreOne_ShouldReturnOnlySecond()
    {
        var owner = new User
        {
            Name = "Peter",
            LastName = "Doe",
            Email = "Peter@gmail.com",
            Password = "Password-1",
            Role = new Role("CompanyOwner"),
            CreatedAt = DateTime.Now,
            HasCompany = false
        };
        var expectedCompany2 = new Company
        {
            Id = Guid.Parse("1a2b3c4d-1234-1234-1234-123456789999"),
            Name = "IBM",
            Owner = owner,
            Rut = "1234567890-9",
            Logo = "logo"
        };

        _companyRepository.Add(_company);
        _companyRepository.Add(expectedCompany2);
        _context.SaveChanges();

        List<Company> companiesSaved = _companyRepository.GetAll(null, 1, 1);

        companiesSaved.Count.Should().Be(1);

        Company companySaved = companiesSaved[0];
        companySaved.Id.Should().Be(expectedCompany2.Id);
        companySaved.Name.Should().Be(expectedCompany2.Name);
    }

    #endregion

    #endregion

    #endregion
}
