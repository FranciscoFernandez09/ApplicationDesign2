using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

namespace SmartHome.BusinessLogic.Tests.DomainTests.SmartDevicesTests;

[TestClass]
public class SmartDeviceTest
{
    private const string Name = "name";
    private const string Model = "AAA111";
    private const string Description = "d1";
    private const string DeviceType = "MotionSensor";

    private static readonly List<DeviceImage> DeviceImages = [new DeviceImage("url1.png", true)];

    private static readonly User User =
        new(new CreateUserArgs("John", "Doe", "johnDoe@gmail.com", "Password1@!", "some.jpg")
        {
            Role = new Role("HomeOwner")
        });

    private readonly Company _validCompany = new(new CreateCompanyArgs("company", User, "1234567890-1", "logo.png",
        Guid.Parse("f3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b")));

    #region CreateSmartDevice

    #region Error

    [TestMethod]
    [DataRow("in#!@!", Description, "name")]
    [DataRow(Name, "desc#!@", "description")]
    public void CreateSmartDevice_WhenInvalidStringParameters_ShouldThrowException(string name, string description,
        string invalidParamName)
    {
        var args = new CreateSmartDeviceArgs(name, Model, description, _validCompany, DeviceType, DeviceImages);

        Func<SmartDevice> act = () => new SmartDevice(args);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage($"Invalid {invalidParamName}: Only letters, numbers and spaces are allowed.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateSmartDevice_WhenValidParameters_ShouldCreateSmartDevice()
    {
        const string validName = "name";
        var args = new CreateSmartDeviceArgs(validName, Model, Description, _validCompany,
            DeviceType, DeviceImages);

        var smartDevice = new SmartDevice(args);

        smartDevice.Id.Should().NotBeEmpty();
        smartDevice.Name.Should().Be(args.Name);
        smartDevice.Model.Should().Be(args.Model);
        smartDevice.Description.Should().Be(args.Description);
        smartDevice.CreateOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(100));
        smartDevice.CompanyOwner.Should().Be(_validCompany);
        smartDevice.CompanyOwnerId.Should().Be(_validCompany.Id);
        smartDevice.Images.Should().BeEquivalentTo(args.Images);
        smartDevice.DeviceType.Should().Be(DeviceTypeEnum.MotionSensor);
        smartDevice.HomeDevices.Should().BeEmpty();
    }

    #endregion

    #endregion
}
