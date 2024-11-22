using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Requests.Filters;

namespace SmartHome.WebApi.Tests.Controllers;

[TestClass]
public class AdminControllerTest
{
    private const string Name = "Peter";
    private const string LastName = "Parker";
    private const string Email = "peter@gmail.com";
    private const string Password = "Password-12";
    private const string ProfileImage = "profileImage.png";
    private const int Offset = 0;
    private const int Limit = 10;
    private Mock<IAdminService> _adminService = null!;
    private AdminController _controller = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _adminService = new Mock<IAdminService>();
        _controller = new AdminController(_adminService.Object);
    }

    #region DeleteAdmin

    [TestMethod]
    public void DeleteAdmin_WhenAdminDeleted_ShouldResponseOk()
    {
        var deleteId = Guid.NewGuid();
        _adminService.Setup(x => x.DeleteAdmin(deleteId));

        IActionResult result = _controller.DeleteAdmin(deleteId);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Admin deleted successfully.");
    }

    #endregion

    #region ChangeRoleToAdminHomeOwner

    [TestMethod]
    public void ChangeRoleToAdminHomeOwner_WhenRoleChanged_ShouldResponseOk()
    {
        var user = new User { Id = Guid.NewGuid() };
        var mockHttpContext = new Mock<HttpContext>();
        var items = new Dictionary<object, object> { { Item.UserLogged, user } };
        mockHttpContext.Setup(c => c.Items).Returns(items!);

        var controllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

        _controller.ControllerContext = controllerContext;

        _adminService.Setup(x => x.ChangeRoleToAdminHomeOwner(user));

        IActionResult result = _controller.ChangeRoleToAdminHomeOwner();

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("You change your role to AdminHomeOwnerRole.");
    }

    #endregion

    #region CreatedAdmin

    #region Error

    [TestMethod]
    [DataRow(null, "Doe", "JohnDoe@gmail.com", "Password--12", null, "name")]
    [DataRow("", "Doe", "JohnDoe@gmail.com", "Password--12", null, "name")]
    [DataRow("John", null, "JohnDoe@gmail.com", "Password--12", null, "lastName")]
    [DataRow("John", "", "JohnDoe@gmail.com", "Password--12", null, "lastName")]
    [DataRow("John", "Doe", null, "Password--12", null, "email")]
    [DataRow("John", "Doe", "", "Password--12", null, "email")]
    [DataRow("John", "Doe", "JohnDoe@gmail.com", null, null, "password")]
    [DataRow("John", "Doe", "JohnDoe@gmail.com", "", null, "password")]
    public void CreateAdmin_WhenHaveParametersNullOrEmpty_ShouldThrowNullException(string name, string lastName,
        string email, string password, string? profileImage, string invalidParamName)
    {
        var request = new CreateUserRequest(name, lastName, email, password, profileImage);

        Func<IActionResult> act = () => _controller.CreateAdmin(request);

        act.Should().ThrowExactly<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{invalidParamName}')");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateAdmin_WhenAdminCreated_ShouldResponseOk()
    {
        var request = new CreateUserRequest(Name, LastName, Email, Password, ProfileImage);
        var args = new CreateUserArgs(Name, LastName, Email, Password, ProfileImage);

        _adminService.Setup(x => x.CreateAdmin(args));

        IActionResult result = _controller.CreateAdmin(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Admin created successfully.");
    }

    #endregion

    #endregion

    #region CreateCompanyOwner

    #region Error

    [TestMethod]
    [DataRow(null, "Doe", "JohnDoe@gmail.com", "Password--12", null, "name")]
    [DataRow("", "Doe", "JohnDoe@gmail.com", "Password--12", null, "name")]
    [DataRow("John", null, "JohnDoe@gmail.com", "Password--12", null, "lastName")]
    [DataRow("John", "", "JohnDoe@gmail.com", "Password--12", null, "lastName")]
    [DataRow("John", "Doe", null, "Password--12", null, "email")]
    [DataRow("John", "Doe", "", "Password--12", null, "email")]
    [DataRow("John", "Doe", "JohnDoe@gmail.com", null, null, "password")]
    [DataRow("John", "Doe", "JohnDoe@gmail.com", "", null, "password")]
    public void CreateCompanyOwner_WhenHaveParametersNullOrEmpty_ShouldThrowNullException(string name, string lastName,
        string email, string password, string? profileImage, string invalidParamName)
    {
        var request = new CreateUserRequest(name, lastName, email, password, profileImage);

        Func<IActionResult> act = () => _controller.CreateCompanyOwner(request);

        act.Should().ThrowExactly<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{invalidParamName}')");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateCompanyOwner_WhenCompanyOwnerCreated_ShouldResponseOk()
    {
        var request = new CreateUserRequest(Name, LastName, Email, Password, ProfileImage);
        var args = new CreateUserArgs(Name, LastName, Email, Password, ProfileImage);

        _adminService.Setup(x => x.CreateCompanyOwner(args));

        IActionResult result = _controller.CreateCompanyOwner(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("Company owner created successfully.");
    }

    #endregion

    #endregion

    #region GetUsers

    #region Error

    [TestMethod]
    [DataRow(null, Limit, "offset")]
    [DataRow(Offset, null, "limit")]
    public void GetUsers_WhenOffsetOrLimitAreNull_ShouldThrowArgumentNullException(int? offset, int? limit,
        string invalidParamName)
    {
        var request = new FilterUserRequest(null, null, null, offset, limit);

        Func<IActionResult> act = () => _controller.GetUsers(request);

        act.Should().ThrowExactly<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{invalidParamName}')");
    }

    [TestMethod]
    public void GetUsers_WhenOffsetIsNegative_ShouldThrowArgumentException()
    {
        const int invalidOffset = -1;
        var request = new FilterUserRequest(null, null, null, invalidOffset, Limit);

        Func<IActionResult> act = () => _controller.GetUsers(request);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Offset must be greater or equal than 0.");
    }

    [TestMethod]
    public void GetUsers_WhenLimitIsNegativeOrZero_ShouldThrowArgumentException()
    {
        const int invalidLimit = 0;
        var request = new FilterUserRequest(null, null, null, Offset, invalidLimit);

        Func<IActionResult> act = () => _controller.GetUsers(request);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Limit must be greater than 0.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetUsers_WhenEmptyUsers_ShouldResponseOk()
    {
        _adminService.Setup(x => x.GetUsers(It.IsAny<FilterUserArgs>())).Returns([]);

        var request = new FilterUserRequest(Name, LastName, null, Offset, Limit);
        IActionResult result = _controller.GetUsers(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeEquivalentTo(new List<ShowUserDto>());
    }

    [TestMethod]
    public void GetUsers_WhenUsersExists_ShouldResponseOk()
    {
        List<ShowUserDto> users =
        [
            new ShowUserDto(Guid.NewGuid(), Name, LastName, "Admin", DateTime.Now),
            new ShowUserDto(Guid.NewGuid(), Name, LastName, "Admin", DateTime.Now)
        ];
        _adminService.Setup(x => x.GetUsers(It.IsAny<FilterUserArgs>())).Returns(users);

        var request = new FilterUserRequest(Name, LastName, null, Offset, Limit);
        IActionResult result = _controller.GetUsers(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeEquivalentTo(users);
    }

    #endregion

    #endregion

    #region GetCompanies

    #region Error

    [TestMethod]
    [DataRow(null, Limit, "offset")]
    [DataRow(Offset, null, "limit")]
    public void GetCompanies_WhenOffsetOrLimitAreNull_ShouldThrowArgumentNullException(int? offset, int? limit,
        string invalidParamName)
    {
        var request = new FilterCompanyRequest(null, null, null, offset, limit);

        Func<IActionResult> act = () => _controller.GetCompanies(request);

        act.Should().ThrowExactly<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{invalidParamName}')");
    }

    [TestMethod]
    public void GetCompanies_WhenOffsetIsNegative_ShouldThrowArgumentException()
    {
        const int invalidOffset = -1;
        var request = new FilterCompanyRequest(null, null, null, invalidOffset, Limit);

        Func<IActionResult> act = () => _controller.GetCompanies(request);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Offset must be greater or equal than 0.");
    }

    [TestMethod]
    public void GetCompanies_WhenLimitIsNegativeOrZero_ShouldThrowArgumentException()
    {
        const int invalidLimit = 0;
        var request = new FilterCompanyRequest(null, null, null, Offset, invalidLimit);

        Func<IActionResult> act = () => _controller.GetCompanies(request);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Limit must be greater than 0.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetCompanies_WhenEmptyCompanies_ShouldResponseOk()
    {
        var companies = new List<ShowCompanyDto>();
        var request = new FilterCompanyRequest(null, Name, LastName, Offset, Limit);

        _adminService.Setup(x => x.GetCompanies(It.IsAny<FilterCompanyArgs>())).Returns(companies);

        IActionResult result = _controller.GetCompanies(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeEquivalentTo(companies);
    }

    [TestMethod]
    public void GetCompanies_WhenCompaniesExists_ShouldResponseOk()
    {
        List<ShowCompanyDto> companies =
        [
            new ShowCompanyDto(Guid.NewGuid(), Name, $"{Name} {LastName}", Email, "123456789"),
            new ShowCompanyDto(Guid.NewGuid(), Name, $"{Name} {LastName}", Email, "123456789")
        ];
        _adminService.Setup(x => x.GetCompanies(It.IsAny<FilterCompanyArgs>())).Returns(companies);

        var request = new FilterCompanyRequest(null, Name, LastName, Offset, Limit);
        IActionResult result = _controller.GetCompanies(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().BeEquivalentTo(companies);
    }

    #endregion

    #endregion
}
