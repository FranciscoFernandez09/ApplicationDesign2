using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

namespace SmartHome.BusinessLogic.Tests.DomainTests;

[TestClass]
public class CompanyTest
{
    private const string CompanyName = "SpaceX";
    private const string CompanyRut = "0123456789-1";
    private const string CompanyLogo = "image.png";
    private readonly Guid _validatorId = Guid.Parse("f3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b");
    private User _validUser = null!;

    [TestInitialize]
    public void Initialize()
    {
        const string validUserName = "John";
        const string validUserLastName = "Doe";
        const string validUserEmail = "JohnDoe@gmail.com";
        const string validPassword = "Password--12";
        const string validProfileImage = "image.png";
        const string validRoleName = "CompanyOwner";

        var validRole = new Role(validRoleName);
        _validUser =
            new User(new CreateUserArgs(validUserName, validUserLastName, validUserEmail, validPassword,
                validProfileImage)
            { Role = validRole });
    }

    #region Success

    [TestMethod]
    public void Company_WhenValidArgs_ShouldCreateCompany()
    {
        var args = new CreateCompanyArgs(CompanyName, _validUser, CompanyRut, CompanyLogo, _validatorId);

        var company = new Company(args);

        company.Id.Should().NotBeEmpty();
        company.Name.Should().Be(CompanyName);
        company.Owner.Should().Be(_validUser);
        company.OwnerId.Should().Be(_validUser.Id);
        company.Rut.Should().Be(CompanyRut);
        company.Logo.Should().Be(CompanyLogo);
        company.ValidatorId.Should().Be(_validatorId);
    }

    #endregion

    #region Error

    [TestMethod]
    public void Company_WhenCreatedWithInvalidName_ShouldThrowArgumentException()
    {
        const string invalidName = "SpaceX%^&*()-_=+[{]};:'|,<.>/?";
        var args = new CreateCompanyArgs(invalidName, _validUser, CompanyRut, CompanyLogo, _validatorId);

        Func<Company> act = () => new Company(args);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Invalid company name: Only letters, numbers and spaces are allowed.");
    }

    [TestMethod]
    [DataRow("01234567890--1")]
    [DataRow("01234567890-")]
    [DataRow("01234567890")]
    [DataRow("012-1")]
    [DataRow("01234567890-11")]
    public void Company_WhenCreatedWithInvalidRUT_ShouldThrowArgumentException(string invalidRut)
    {
        var args = new CreateCompanyArgs(CompanyName, _validUser, invalidRut, CompanyLogo, _validatorId);

        Func<Company> act = () => new Company(args);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Invalid RUT: Format is 10 numbers - 1 number (XXXXXXXXXX-X)");
    }

    [TestMethod]
    [DataRow("image.pngg")]
    [DataRow("imagepng")]
    [DataRow("imagejpg.")]
    [DataRow(".jpg")]
    public void Company_WhenCreatedWithInvalidLogo_ShouldThrowArgumentException(string invalidLogo)
    {
        var args = new CreateCompanyArgs(CompanyName, _validUser, CompanyRut, invalidLogo, _validatorId);

        Func<Company> act = () => new Company(args);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Invalid logo: Format should be <image-name>.<png/jpg>");
    }

    #endregion
}
