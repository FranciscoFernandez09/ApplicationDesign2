using FluentAssertions;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Tests.DomainTests;

[TestClass]
public class SystemPermissionTest
{
    private const string SystemPermissionName = "CreateAdmin";

    #region Success

    [TestMethod]
    public void SystemPermission_WhenParametersAreValid_ShouldCreateRole()
    {
        var systemPermission = new SystemPermission(SystemPermissionName);

        systemPermission.Id.Should().NotBeEmpty();
        systemPermission.Name.Should().Be(SystemPermissionName);
    }

    #endregion

    #region Error

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void SystemPermission_WhenNameIsNullOrEmpty_ShouldThrowArgumentNullException(
        string invalidSystemPermissionName)
    {
        Func<SystemPermission> act = () => new SystemPermission(invalidSystemPermissionName);

        act.Should().ThrowExactly<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'name')");
    }

    [TestMethod]
    public void SystemPermission_WhenNameIsInvalid_ShouldThrowArgumentException()
    {
        const string invalidSystemPermissionName = "CreateAdmin!@#$%^&*()-_=+";
        Func<SystemPermission> act = () => new SystemPermission(invalidSystemPermissionName);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Invalid system permission name: Only letters, numbers and spaces are allowed.");
    }

    #endregion
}
