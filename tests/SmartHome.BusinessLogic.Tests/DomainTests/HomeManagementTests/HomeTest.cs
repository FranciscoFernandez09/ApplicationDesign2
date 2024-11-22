using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

namespace SmartHome.BusinessLogic.Tests.DomainTests.HomeManagementTests;

[TestClass]
public class HomeTest
{
    private const string AddressStreet = "street";
    private const int AddressNumber = 123;
    private const int Latitude = 123;
    private const int Longitude = 123;
    private const int MaxMembers = 10;
    private const string HomeName = "name";

    private readonly User _validOwner =
        new(new CreateUserArgs("name", "lastName", "user@gmail.com", "passwordD123-!", null)
        {
            Role = new Role("Admin")
        });

    #region Success

    [TestMethod]
    public void CreateHome_WhenValidParameters_ShouldCreateHome()
    {
        var home = new Home(new CreateHomeArgs(AddressStreet, AddressNumber, Latitude, Longitude,
            MaxMembers, HomeName, _validOwner));

        home.AddressStreet.Should().Be(AddressStreet);
        home.AddressNumber.Should().Be(AddressNumber);
        home.Latitude.Should().Be(Latitude);
        home.Longitude.Should().Be(Longitude);
        home.MaxMembers.Should().Be(MaxMembers);
        home.Owner.Should().Be(_validOwner);
        home.OwnerId.Should().Be(_validOwner.Id);
        home.Name.Should().Be(HomeName);
        home.Devices.Should().BeEmpty();
        home.Members.Should().BeEmpty();
        home.Rooms.Should().BeEmpty();
    }

    #endregion

    #region Error

    [TestMethod]
    public void CreateHome_WhenInvalidAddressStreet_ShouldThrowException()
    {
        var createHomeArgs = new CreateHomeArgs("street#!2", AddressNumber, Latitude, Longitude,
            MaxMembers, HomeName, _validOwner);
        Func<Home> act = () => new Home(createHomeArgs);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Invalid street: Only letters and spaces are allowed.");
    }

    [TestMethod]
    public void CreateHome_WhenAddressNumberIsInvalid_ShouldThrowException()
    {
        var createHomeArgs = new CreateHomeArgs("street", 0, Latitude, Longitude,
            MaxMembers, HomeName, _validOwner);
        Func<Home> act = () => new Home(createHomeArgs);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Invalid address number: Must be greater than 0.");
    }

    [TestMethod]
    public void CreateHome_WhenMaxMembersIsInvalid_ShouldThrowException()
    {
        var createHomeArgs = new CreateHomeArgs(AddressStreet, AddressNumber, Latitude, Longitude,
            0, HomeName, _validOwner);
        Func<Home> act = () => new Home(createHomeArgs);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Invalid max members: Must be greater than 0.");
    }

    [TestMethod]
    public void CreateHome_WhenLatitudeIsInvalid_ShouldThrowException()
    {
        var createHomeArgs = new CreateHomeArgs(AddressStreet, AddressNumber, -91, Longitude,
            MaxMembers, HomeName, _validOwner);
        Func<Home> act = () => new Home(createHomeArgs);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Invalid latitude: Must be greater than 0.");
    }

    [TestMethod]
    public void CreateHome_WhenLongitudeIsInvalid_ShouldThrowException()
    {
        var createHomeArgs = new CreateHomeArgs(AddressStreet, AddressNumber, Latitude, -181,
            MaxMembers, HomeName, _validOwner);
        Func<Home> act = () => new Home(createHomeArgs);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Invalid longitude: Must be greater than 0.");
    }

    #endregion
}
