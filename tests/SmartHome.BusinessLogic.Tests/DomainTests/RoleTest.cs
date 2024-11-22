using FluentAssertions;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Tests.DomainTests;

[TestClass]
public class RoleTest
{
    private const string RoleName = "Admin";
    private const string SystemPermissionName = "CreateAdmin";
    private readonly Role _role = new(RoleName);

    #region CreateRole

    #region Error

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void Role_WhenNameIsNullOrEmpty_ShouldThrowArgumentNullException(string invalidRoleName)
    {
        Func<Role> act = () => new Role(invalidRoleName);

        act.Should().ThrowExactly<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'name')");
    }

    [TestMethod]
    public void Role_WhenNameIsInvalid_ShouldThrowArgumentException()
    {
        const string invalidRoleName = "Admin!@#$%^&*()-_=+[";
        Func<Role> act = () => new Role(invalidRoleName);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Invalid role name: Only letters, numbers and spaces are allowed.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Role_WhenValidParameters_ShouldCreateRole()
    {
        var role = new Role(RoleName);

        role.Id.Should().NotBeEmpty();
        role.Name.Should().Be(RoleName);
    }

    #endregion

    #endregion

    #region PermissionsManagement

    [TestMethod]
    public void Permissions_WhenRoleIsCreated_ShouldBeEmpty()
    {
        _role.Permissions.Should().BeEmpty();
    }

    [TestMethod]
    public void AddPermission_WhenPermissionsAreAdded_ShouldAddPermissions()
    {
        var systemPermission = new SystemPermission(SystemPermissionName);
        _role.AddPermission(systemPermission);

        _role.Permissions.Should().Contain(systemPermission);
    }

    #endregion
}
