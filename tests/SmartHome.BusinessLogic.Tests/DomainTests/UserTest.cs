using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

namespace SmartHome.BusinessLogic.Tests.DomainTests;

[TestClass]
public class UserTest
{
    private const string Name = "John";
    private const string LastName = "Doe";
    private const string Email = "JohnDoe@gmail.com";
    private const string Password = "Password--12";
    private const string ProfileImage = "";
    private const string AdminRoleName = "Admin";

    private readonly Role _validRole = new(AdminRoleName);

    #region ToString

    [TestMethod]
    public void ToString_WhenCalled_ShouldReturnString()
    {
        var args =
            new CreateUserArgs(Name, LastName, Email, Password, ProfileImage) { Role = _validRole };
        var user = new User(args);

        var result = user.ToString();

        result.Should()
            .Be(
                $"Name: {Name} - LastName: {LastName} - Email: {Email} - ProfileImage: {ProfileImage}");
    }

    #endregion

    #region CreateUser

    #region Error

    [TestMethod]
    [DataRow("John!@#$%^&*()_+-=.,<>?/;:[]{}|")]
    [DataRow("John1")]
    public void User_WhenInvalidName_ShouldThrowException(string invalidName)
    {
        var args =
            new CreateUserArgs(invalidName, LastName, Email, Password, ProfileImage) { Role = _validRole };
        Func<User> act = () => new User(args);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Invalid name: Only letters and spaces are allowed.");
    }

    [TestMethod]
    [DataRow("Doe1")]
    [DataRow("Doe!@#$%^&*()_+-=.,<>?/;:[]{}|")]
    public void User_WhenLastNameIsInvalid_ShouldBeException(string invalidLastName)
    {
        var args =
            new CreateUserArgs(Name, invalidLastName, Email, Password, ProfileImage) { Role = _validRole };

        Func<User> act = () => new User(args);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Invalid last name: Only letters and spaces are allowed.");
    }

    [TestMethod]
    [DataRow("JohnDoe.com")]
    [DataRow("JohnDoe@com")]
    [DataRow("@gmail.com")]
    [DataRow("JohnDoe@")]
    public void User_WhenEmailIsInvalid_ShouldBeException(string invalidEmail)
    {
        var args =
            new CreateUserArgs(Name, LastName, invalidEmail, Password, ProfileImage) { Role = _validRole };
        Func<User> act = () => new User(args);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Invalid email: Must have an special character '@', a domain and letters before the '@'.");
    }

    [TestMethod]
    [DataRow("Pa123!")]
    [DataRow("Password!")]
    [DataRow("Password123")]
    [DataRow("password123!")]
    [DataRow("PASSWORD123!")]
    public void User_WhenPasswordHasLessThan8Characters_ShouldBeException(string invalidPassword)
    {
        var args =
            new CreateUserArgs(Name, LastName, Email, invalidPassword, ProfileImage) { Role = _validRole };
        Func<User> act = () => new User(args);

        act.Should().ThrowExactly<ArgumentException>().WithMessage(
            "Invalid password: Must be at least 8 characters long and include numbers, special character, uppercase and lowercase letters.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void User_WhenValidArgs_ShouldCreateUser()
    {
        var args =
            new CreateUserArgs(Name, LastName, Email, Password, ProfileImage) { Role = _validRole };
        var user = new User(args);

        user.Id.Should().NotBeEmpty();
        user.Name.Should().Be(Name);
        user.LastName.Should().Be(LastName);
        user.Email.Should().Be(Email);
        user.Password.Should().Be(Password);
        user.Role.Should().Be(_validRole);
        user.RoleId.Should().Be(_validRole.Id);
        user.ProfileImage.Should().Be(ProfileImage);
        user.HasCompany.Should().BeFalse();
        user.CreatedAt.Should().BeCloseTo(DateTimeProvider.Now, TimeSpan.FromMilliseconds(100));
        user.HomeMembers.Should().BeEmpty();
    }

    #endregion

    #endregion
}
