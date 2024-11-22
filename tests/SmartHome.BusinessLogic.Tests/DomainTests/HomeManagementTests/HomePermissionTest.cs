using FluentAssertions;
using SmartHome.BusinessLogic.Domain.HomeManagement;

namespace SmartHome.BusinessLogic.Tests.DomainTests.HomeManagementTests;

[TestClass]
public class HomePermissionTest
{
    private const string HomePermissionName = "CreateAdmin";

    #region ToString

    [TestMethod]
    public void ToString_WhenCalled_ShouldReturnString()
    {
        var homePermission = new HomePermission(HomePermissionName);

        var result = homePermission.ToString();

        result.Should().Be($"Permission: {HomePermissionName}");
    }

    #endregion

    #region HomePermission

    #region Error

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void HomePermission_WhenNameIsNullOrEmpty_ShouldThrowArgumentNullException(string invalidName)
    {
        Func<HomePermission> act = () => new HomePermission(invalidName);

        act.Should().ThrowExactly<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'name')");
    }

    [TestMethod]
    public void HomePermission_WhenNameIsInvalid_ShouldThrowArgumentException()
    {
        const string invalidHomePermissionName = "CreateAdmin!@#$%^&*()-_=+";
        Func<HomePermission> act = () => new HomePermission(invalidHomePermissionName);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Invalid home permission name: Only letters, numbers and spaces are allowed.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void HomePermission_WhenParametersAreValid_ShouldCreateRole()
    {
        var homePermission = new HomePermission(HomePermissionName);

        homePermission.Id.Should().NotBeEmpty();
        homePermission.Name.Should().Be(HomePermissionName);
    }

    #endregion

    #endregion
}
