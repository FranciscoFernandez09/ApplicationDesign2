using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogic.Tests.ServicesTests;

[TestClass]
public class AdminServiceTest
{
    private const string Name = "John";
    private const string LastName = "Doe";
    private const string Email = "JohnDoe@gmail.com";
    private const string Password = "Password--12";
    private const string ProfileImage = "";
    private const string AdminRoleName = "Admin";
    private const string CompanyOwnerRoleName = "CompanyOwner";

    private const string CompanyName = "Company";
    private const string Rut = "1234567890-1";
    private const string Logo = "Logo.png";
    private const int Offset = 0;
    private const int Limit = 10;
    private readonly Guid _validatorId = Guid.Parse("f3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b");
    private User _companyOwner = null!;
    private Mock<IRepository<Company>> _companyRepositoryMock = null!;
    private Mock<IRepository<Role>> _roleRepositoryMock = null!;
    private AdminService _service = null!;
    private Mock<IRepository<User>> _userRepositoryMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Strict);
        _roleRepositoryMock = new Mock<IRepository<Role>>(MockBehavior.Strict);
        _companyRepositoryMock = new Mock<IRepository<Company>>(MockBehavior.Strict);

        _roleRepositoryMock.Setup(rs => rs.Add(It.IsAny<Role>())).Verifiable();
        _service = new AdminService(_userRepositoryMock.Object, _roleRepositoryMock.Object,
            _companyRepositoryMock.Object);

        _companyOwner =
            new User(new CreateUserArgs(Name, LastName, Email, Password, ProfileImage)
            {
                Role = new Role(CompanyOwnerRoleName)
            });
    }

    #region ChangeRoleToAdminHomeOwner

    [TestMethod]
    public void ChangeRoleToAdminHomeOwner_WhenIsValid_ShouldChangeRoleToAdminHomeOwner()
    {
        var user = new User(new CreateUserArgs(Name, LastName, Email, Password, ProfileImage)
        {
            Role = new Role { Id = Constant.AdminRoleId, Name = "Admin" }
        });
        var expectedRole = new Role { Id = Constant.AdminHomeOwnerRoleId, Name = "AdminHomeOwner" };

        _roleRepositoryMock.Setup(rs => rs.Get(r => r.Id == Constant.AdminHomeOwnerRoleId)).Returns(expectedRole);
        _userRepositoryMock.Setup(us => us.Update(user)).Verifiable();

        _service.ChangeRoleToAdminHomeOwner(user);

        user.Role.Should().Be(expectedRole);
        _userRepositoryMock.Verify(us => us.Update(user), Times.Once);
        _userRepositoryMock.VerifyAll();
    }

    #endregion

    #region AdminManagement

    #region CreateAdmin

    #region Error

    [TestMethod]
    public void CreateAdmin_WhenEmailAlreadyExists_ShouldThrowException()
    {
        _roleRepositoryMock.Setup(rs => rs.Get(r => r.Id == Constant.AdminRoleId))
            .Returns(new Role(AdminRoleName));
        _userRepositoryMock.Setup(us => us.Exists(u => u.Email == Email)).Returns(true);

        var args = new CreateUserArgs(Name, LastName, Email, Password,
            ProfileImage);
        Action act = () => _service.CreateAdmin(args);

        act.Should().Throw<InvalidOperationException>().WithMessage("Email already exists.");
        _userRepositoryMock.VerifyAll();
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateAdmin_WhenValidArgs_ShouldCreateAdmin()
    {
        _roleRepositoryMock.Setup(rs => rs.Get(r => r.Id == Constant.AdminRoleId))
            .Returns(new Role(AdminRoleName));
        _userRepositoryMock.Setup(us => us.Exists(u => u.Email == Email)).Returns(false);
        _userRepositoryMock.Setup(us => us.Add(It.IsAny<User>())).Verifiable();

        var args = new CreateUserArgs(Name, LastName, Email, Password,
            ProfileImage);
        _service.CreateAdmin(args);

        _userRepositoryMock.Verify(us => us.Add(It.Is<User>(u =>
            u.Id != Guid.Empty &&
            u.Name == Name &&
            u.LastName == LastName &&
            u.Email == Email &&
            u.Password == Password &&
            u.Role.Name == AdminRoleName &&
            u.ProfileImage == ProfileImage)), Times.Once);
        _userRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

    #region DeleteAdmin

    #region Error

    [TestMethod]
    public void DeleteAdmin_WhenIdIsNull_ShouldThrowArgumentNullException()
    {
        Action act = () => _service.DeleteAdmin(Guid.Empty);

        act.Should().ThrowExactly<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'deleteId')");
    }

    [TestMethod]
    public void DeleteAdmin_WhenAdminNotExists_ShouldThrowException()
    {
        var deleteId = Guid.NewGuid();

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == deleteId)).Returns((User?)null);

        Action act = () => _service.DeleteAdmin(deleteId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Admin not exists.");
        _userRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void DeleteAdmin_WhenUserIsNotAdmin_ShouldThrowException()
    {
        var user = new User(new CreateUserArgs(Name, LastName, Email, Password, ProfileImage)
        {
            Role = new Role { Id = Constant.CompanyOwnerRoleId, Name = "CompanyOwner" }
        });
        Guid deleteId = user.Id;

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == deleteId)).Returns(user);

        Action act = () => _service.DeleteAdmin(deleteId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Admin not exists.");
        _userRepositoryMock.VerifyAll();
    }

    #endregion

    #region Success

    [TestMethod]
    public void DeleteAdmin_WhenExists_ShouldDeleteAdmin()
    {
        var user = new User(new CreateUserArgs(Name, LastName, Email, Password, ProfileImage)
        {
            Role = new Role { Id = Constant.AdminRoleId, Name = "Admin" }
        });
        Guid deleteId = user.Id;

        _userRepositoryMock.Setup(us => us.Get(u => u.Id == deleteId)).Returns(user);
        _userRepositoryMock.Setup(us => us.Delete(user)).Verifiable();

        _service.DeleteAdmin(deleteId);

        _userRepositoryMock.Verify(us => us.Delete(user), Times.Once);
        _userRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

    #endregion

    #region CreateCompanyOwner

    #region Error

    [TestMethod]
    public void CreateCompanyOwner_WhenEmailAlreadyExists_ShouldThrowException()
    {
        var args = new CreateUserArgs(Name, LastName, Email, Password,
            ProfileImage);

        _roleRepositoryMock.Setup(rs => rs.Get(r => r.Id == Constant.CompanyOwnerRoleId))
            .Returns(new Role(CompanyOwnerRoleName));
        _userRepositoryMock.Setup(us => us.Exists(u => u.Email == Email)).Returns(true);

        Action act = () =>
            _service.CreateCompanyOwner(args);

        act.Should().Throw<InvalidOperationException>().WithMessage("Email already exists.");
        _userRepositoryMock.VerifyAll();
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateCompanyOwner_WhenValidArgs_ShouldCreateCompanyOwner()
    {
        var args = new CreateUserArgs(Name, LastName, Email, Password,
            ProfileImage);

        _roleRepositoryMock.Setup(rs => rs.Get(r => r.Id == Constant.CompanyOwnerRoleId))
            .Returns(new Role(CompanyOwnerRoleName));
        _userRepositoryMock.Setup(us => us.Exists(u => u.Email == Email)).Returns(false);
        _userRepositoryMock.Setup(us => us.Add(It.IsAny<User>())).Verifiable();

        _service.CreateCompanyOwner(args);

        _userRepositoryMock.Verify(us => us.Add(It.Is<User>(u =>
            u.Name == Name &&
            u.LastName == LastName &&
            u.Email == Email &&
            u.Password == Password &&
            u.Role.Name == CompanyOwnerRoleName &&
            u.ProfileImage == ProfileImage)), Times.Once);
        _userRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

    #region GettersManagement

    #region GetUsers

    #region Success

    [TestMethod]
    public void GetUsers_WhenTheyDontHaveConditions_ShouldReturnAllUsers()
    {
        var users = new List<User>
        {
            new(new CreateUserArgs(Name, LastName, Email, Password, ProfileImage)
            {
                Role = new Role(AdminRoleName)
            }),
            new(new CreateUserArgs(Name, LastName, Email, Password, ProfileImage)
            {
                Role = new Role(CompanyOwnerRoleName)
            }),
            new(new CreateUserArgs(Name, LastName, Email, Password, ProfileImage)
            {
                Role = new Role(AdminRoleName)
            })
        };
        var dto = new FilterUserArgs(null, null, null, Offset, Limit);
        var expected = users.Select(MapUserToShowUserDto).ToList();

        _userRepositoryMock.Setup(us => us.GetAll(u =>
            (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.LastName) ||
             (u.Name.Contains(dto.Name) && u.LastName.Contains(dto.LastName))) &&
            (string.IsNullOrEmpty(dto.Role) || u.Role.Name == dto.Role), Offset, Limit)).Returns(users);

        List<ShowUserDto> result = _service.GetUsers(dto);

        result.Should().BeEquivalentTo(expected);
        _userRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void GetUsers_WhenTheyHaveRoleConditions_ShouldReturnFilteredUsers()
    {
        var users = new List<User>
        {
            new(new CreateUserArgs(Name, LastName, Email, Password, ProfileImage)
            {
                Role = new Role(AdminRoleName)
            }),
            new(new CreateUserArgs(Name, LastName, Email, Password, ProfileImage)
            {
                Role = new Role(CompanyOwnerRoleName)
            }),
            new(new CreateUserArgs(Name, LastName, Email, Password, ProfileImage)
            {
                Role = new Role(AdminRoleName)
            })
        };

        var filteredUsers = users.Where(u => u.Role.Name == AdminRoleName).ToList();
        var expected = filteredUsers.Select(MapUserToShowUserDto).ToList();
        var dto = new FilterUserArgs(null, null, AdminRoleName, Offset, Limit);

        _userRepositoryMock.Setup(us => us.GetAll(u =>
                (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.LastName) ||
                 (u.Name.Contains(dto.Name) && u.LastName.Contains(dto.LastName))) &&
                (string.IsNullOrEmpty(dto.Role) || u.Role.Name == dto.Role), Offset, Limit))
            .Returns(filteredUsers);

        List<ShowUserDto> result = _service.GetUsers(dto);

        result.Should().BeEquivalentTo(expected);
        _userRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void GetUsers_WhenTheyHaveNameConditions_ShouldReturnFilteredUsers()
    {
        const string name = "Mia";
        const string lastName = "Doe";
        var users = new List<User>
        {
            new(new CreateUserArgs(Name, LastName, Email, Password, ProfileImage)
            {
                Role = new Role(AdminRoleName)
            }),
            new(new CreateUserArgs(Name, LastName, Email, Password, ProfileImage)
            {
                Role = new Role(CompanyOwnerRoleName)
            }),
            new(new CreateUserArgs(Name, LastName, Email, Password, ProfileImage)
            {
                Role = new Role(AdminRoleName)
            })
        };

        var filteredUsers = users.Where(u => u is { Name: name, LastName: lastName }).ToList();
        var expected = filteredUsers.Select(MapUserToShowUserDto).ToList();
        var dto = new FilterUserArgs(name, lastName, null, Offset, Limit);

        _userRepositoryMock.Setup(us => us.GetAll(u =>
                (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.LastName) ||
                 (u.Name.Contains(dto.Name) && u.LastName.Contains(dto.LastName))) &&
                (string.IsNullOrEmpty(dto.Role) || u.Role.Name == dto.Role), Offset, Limit))
            .Returns(filteredUsers);

        List<ShowUserDto> result = _service.GetUsers(dto);

        result.Should().BeEquivalentTo(expected);
        _userRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void GetUsers_WhenNoUsers_ShouldReturnEmptyList()
    {
        var dto = new FilterUserArgs(null, null, null, Offset, Limit);

        _userRepositoryMock.Setup(us => us.GetAll(u =>
            (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.LastName) ||
             (u.Name.Contains(dto.Name) && u.LastName.Contains(dto.LastName))) &&
            (string.IsNullOrEmpty(dto.Role) || u.Role.Name == dto.Role), Offset, Limit)).Returns([]);

        List<ShowUserDto> result = _service.GetUsers(dto);

        result.Should().BeEmpty();
        _userRepositoryMock.VerifyAll();
    }

    #endregion

    private static ShowUserDto MapUserToShowUserDto(User user)
    {
        return new ShowUserDto(user.Id, user.Name, user.LastName, user.Role.Name, user.CreatedAt);
    }

    #endregion

    #region GetCompanies

    #region Success

    [TestMethod]
    public void GetCompanies_WhenTheyDontHaveConditions_ShouldReturnAllCompanies()
    {
        var companies = new List<Company>
        {
            new(new CreateCompanyArgs(CompanyName, _companyOwner, Rut, Logo, _validatorId)),
            new(new CreateCompanyArgs(CompanyName, _companyOwner, Rut, Logo, _validatorId)),
            new(new CreateCompanyArgs(CompanyName, _companyOwner, Rut, Logo, _validatorId))
        };
        var dto = new FilterCompanyArgs(null, null, null, Offset, Limit);
        var expected = companies.Select(MapCompanyToShowCompanyDto).ToList();

        _companyRepositoryMock.Setup(cs => cs.GetAll(c =>
                (string.IsNullOrEmpty(dto.CompanyName) || c.Name == dto.CompanyName) &&
                (string.IsNullOrEmpty(dto.OwnerName) || string.IsNullOrEmpty(dto.OwnerLastName) ||
                 (c.Owner.Name == dto.OwnerName && c.Owner.LastName == dto.OwnerLastName)), Offset, Limit))
            .Returns(companies);

        List<ShowCompanyDto> result = _service.GetCompanies(dto);

        result.Should().BeEquivalentTo(expected);
        _userRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void GetCompanies_WhenTheyHaveCompanyNameConditions_ShouldReturnFilteredCompanies()
    {
        const string name = "FilteredCompany";
        var companies = new List<Company>
        {
            new(new CreateCompanyArgs(CompanyName, _companyOwner, Rut, Logo, _validatorId)),
            new(new CreateCompanyArgs(name, _companyOwner, Rut, Logo, _validatorId)),
            new(new CreateCompanyArgs(CompanyName, _companyOwner, Rut, Logo, _validatorId))
        };

        var filteredCompanies = companies.Where(c => c.Name == name).ToList();
        var expected = filteredCompanies.Select(MapCompanyToShowCompanyDto).ToList();
        var dto = new FilterCompanyArgs(name, null, null, Offset, Limit);

        _companyRepositoryMock.Setup(cs => cs.GetAll(c =>
                (string.IsNullOrEmpty(dto.CompanyName) || c.Name == dto.CompanyName) &&
                (string.IsNullOrEmpty(dto.OwnerName) || string.IsNullOrEmpty(dto.OwnerLastName) ||
                 (c.Owner.Name == dto.OwnerName && c.Owner.LastName == dto.OwnerLastName)), Offset, Limit))
            .Returns(filteredCompanies);

        List<ShowCompanyDto> result = _service.GetCompanies(dto);

        result.Should().BeEquivalentTo(expected);
        _userRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void GetCompanies_WhenTheyHaveOwnerFullNameConditions_ShouldReturnFilteredCompanies()
    {
        const string name = "FilterName";
        const string lastName = "FilterLastName";
        var owner = new User(
            new CreateUserArgs(name, lastName, Email, Password, ProfileImage)
            {
                Role = new Role(CompanyOwnerRoleName)
            });
        var companies = new List<Company>
        {
            new(new CreateCompanyArgs(CompanyName, _companyOwner, Rut, Logo, _validatorId)),
            new(new CreateCompanyArgs(CompanyName, _companyOwner, Rut, Logo, _validatorId)),
            new(new CreateCompanyArgs(CompanyName, owner, Rut, Logo, _validatorId))
        };

        var filteredCompanies = companies.Where(c => c.Owner is { Name: name, LastName: lastName }).ToList();
        var expected = filteredCompanies.Select(MapCompanyToShowCompanyDto).ToList();
        var dto = new FilterCompanyArgs(name, lastName, null, Offset, Limit);

        _companyRepositoryMock.Setup(cs => cs.GetAll(c =>
                (string.IsNullOrEmpty(dto.CompanyName) || c.Name == dto.CompanyName) &&
                (string.IsNullOrEmpty(dto.OwnerName) || string.IsNullOrEmpty(dto.OwnerLastName) ||
                 (c.Owner.Name == dto.OwnerName && c.Owner.LastName == dto.OwnerLastName)), Offset, Limit))
            .Returns(filteredCompanies);

        List<ShowCompanyDto> result = _service.GetCompanies(dto);

        result.Should().BeEquivalentTo(expected);
        _userRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void GetCompanies_WhenNoCompanies_ShouldReturnEmptyList()
    {
        var dto = new FilterCompanyArgs(null, null, null, Offset, Limit);

        _companyRepositoryMock.Setup(cs => cs.GetAll(c =>
                (string.IsNullOrEmpty(dto.CompanyName) || c.Name == dto.CompanyName) &&
                (string.IsNullOrEmpty(dto.OwnerName) || string.IsNullOrEmpty(dto.OwnerLastName) ||
                 (c.Owner.Name == dto.OwnerName && c.Owner.LastName == dto.OwnerLastName)), Offset, Limit))
            .Returns([]);

        List<ShowCompanyDto> result = _service.GetCompanies(dto);

        result.Should().BeEmpty();
        _userRepositoryMock.VerifyAll();
    }

    #endregion

    private static ShowCompanyDto MapCompanyToShowCompanyDto(Company company)
    {
        return new ShowCompanyDto(company.Id, company.Name, company.GetOwnerFullName(), company.GetOwnerEmail(),
            company.Rut);
    }

    #endregion

    #endregion
}
