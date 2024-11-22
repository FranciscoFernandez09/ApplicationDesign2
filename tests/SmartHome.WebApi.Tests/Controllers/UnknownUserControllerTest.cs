using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Requests;

namespace SmartHome.WebApi.Tests.Controllers;

[TestClass]
public class UnknownUserControllerTest
{
    private UnknownUserController _controller = null!;

    private Mock<IUnknownUserService> _serviceMock = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _serviceMock = new Mock<IUnknownUserService>();
        _controller = new UnknownUserController(_serviceMock.Object);
    }

    #region CreateHomeOwner

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
    public void CreateHomeOwner_WhenRequiredParametersAreNullOrEmpty_ShouldThrowArgumentNullException(string name,
        string lastname, string email, string password, string profileImage, string invalidParamName)
    {
        var request = new CreateUserRequest(name, lastname, email, password, profileImage);

        Func<IActionResult> act = () => _controller.CreateHomeOwner(request);

        act.Should().ThrowExactly<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{invalidParamName}')");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateHomeOwner_WhenUserExists_ShouldReturnOk()
    {
        const string validName = "John";
        const string validLastName = "Doe";
        const string validEmail = "JohnDoe@gmail.com";
        const string validPassword = "Password1!";
        const string validProfileImage = "profile.png";

        var request = new CreateUserRequest(validName, validLastName, validEmail, validPassword, validProfileImage);
        var args = new CreateUserArgs(validName, validLastName, validEmail, validPassword, validProfileImage);

        _serviceMock.Setup(x => x.CreateHomeOwner(args));

        IActionResult result = _controller.CreateHomeOwner(request);

        result.Should().BeOfType<OkObjectResult>();

        var okObjectResult = result as OkObjectResult;
        okObjectResult?.Value.Should().Be("User created successfully.");
    }

    #endregion

    #endregion
}
