using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

namespace SmartHome.BusinessLogic.Tests.DomainTests.HomeManagementTests;

[TestClass]
public class HomeMemberTest
{
    private Home _validHome = null!;
    private HomePermission _validPermission = null!;
    private User _validUser = null!;

    [TestInitialize]
    public void Initialize()
    {
        _validUser =
            new User(new CreateUserArgs("John", "Doe", "johndoe@gmail.com", "Password1@!", null)
            {
                Role = new Role("HomeOwner")
            });
        _validHome = new Home(new CreateHomeArgs("Street", 123, 45, 67, 10, "name", _validUser));
        _validPermission = new HomePermission("AddSmartDevice");
    }

    [TestMethod]
    public void HomeMember_WhenCreated_ShouldInitializeCorrectly()
    {
        var homeMember = new HomeMember(_validHome, _validUser);

        homeMember.Id.Should().NotBeEmpty();
        homeMember.HomeId.Should().Be(_validHome.Id);
        homeMember.UserId.Should().Be(_validUser.Id);
        homeMember.Home.Should().Be(_validHome);
        homeMember.User.Should().Be(_validUser);
        homeMember.ShouldNotify.Should().BeFalse();
        homeMember.Notifications.Should().BeEmpty();
    }

    [TestMethod]
    public void HasHomePermission_WhenPermissionExists_ShouldReturnTrue()
    {
        var homeMember = new HomeMember(_validHome, _validUser);
        homeMember.AddHomePermission(_validPermission);

        var result = homeMember.HasHomePermission(_validPermission.Id);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasHomePermission_WhenPermissionDoesNotExist_ShouldReturnFalse()
    {
        var homeMember = new HomeMember(_validHome, _validUser);

        var result = homeMember.HasHomePermission(Guid.NewGuid());

        result.Should().BeFalse();
    }

    [TestMethod]
    public void AddHomePermission_ShouldAddPermission()
    {
        var homeMember = new HomeMember(_validHome, _validUser);

        homeMember.AddHomePermission(_validPermission);

        homeMember.HasHomePermission(_validPermission.Id).Should().BeTrue();
    }

    [TestMethod]
    public void GetUserData_ShouldReturnCorrectData()
    {
        var homeMember = new HomeMember(_validHome, _validUser);
        homeMember.AddHomePermission(_validPermission);

        var result = homeMember.GetUserData();

        result.Should().ContainInOrder(_validUser.GetFullName(), _validUser.Email, _validUser.ProfileImage,
            _validPermission.Name);
    }

    [TestMethod]
    public void OwnerMemberSettings_ShouldSetOwnerSettings()
    {
        var homeMember = new HomeMember(_validHome, _validUser);

        homeMember.OwnerMemberSettings();

        homeMember.ShouldNotify.Should().BeTrue();
    }
}
