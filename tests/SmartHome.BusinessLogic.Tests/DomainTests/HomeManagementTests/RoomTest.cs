using FluentAssertions;
using SmartHome.BusinessLogic.Domain.HomeManagement;

namespace SmartHome.BusinessLogic.Tests.DomainTests.HomeManagementTests;

[TestClass]
public class RoomTest
{
    [TestMethod]
    public void Room_WhenCreatedWithValidData_ShouldCreated()
    {
        const string name = "Living room";
        var home = new Home { Id = Guid.NewGuid() };

        var room = new Room(name, home);

        room.Id.Should().NotBeEmpty();
        room.Name.Should().Be(name);
        room.Home.Should().Be(home);
        room.Devices.Should().BeEmpty();
    }
}
