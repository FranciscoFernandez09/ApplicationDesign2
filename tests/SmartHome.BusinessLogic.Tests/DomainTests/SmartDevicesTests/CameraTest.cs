using FluentAssertions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

namespace SmartHome.BusinessLogic.Tests.DomainTests.SmartDevicesTests;

[TestClass]
public class CameraTest
{
    private const string Name = "name";
    private const string Model = "AAA111";
    private const bool ExteriorUse = false;
    private const bool InteriorUse = false;
    private const string Description = "d1";
    private const bool MotionDetection = false;
    private const bool PersonDetection = false;

    private static readonly User User =
        new(new CreateUserArgs("John", "Doe", "johndoe@gmail.com", "Password1@!", null)
        {
            Role = new Role("HomeOwner")
        });

    private readonly Company _validCompany = new(new CreateCompanyArgs("company", User, "1234567890-1", "logo.png",
        Guid.Parse("f3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b")));

    private readonly List<DeviceImage> _validDeviceImage = [new DeviceImage("url.jpg", true)];

    [TestMethod]
    public void CreateCamera_WhenValidParameters_ShouldCreateCamera()
    {
        var args = new CreateCameraArgs(Name, Model, Description, _validCompany,
            _validDeviceImage, ExteriorUse, InteriorUse, MotionDetection, PersonDetection);

        var camera = new Camera(args);

        camera.Id.Should().NotBeEmpty();
        camera.Name.Should().Be(args.Name);
        camera.Model.Should().Be(args.Model);
        camera.HasExternalUse.Should().Be(args.HasExternalUse);
        camera.HasInternalUse.Should().Be(args.HasInternalUse);
        camera.Description.Should().Be(args.Description);
        camera.CreateOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(100));
        camera.CompanyOwner.Should().Be(args.CompanyOwner);
        camera.Images.Should().BeEquivalentTo(args.Images);
        camera.MotionDetection.Should().Be(args.MotionDetection);
        camera.PersonDetection.Should().Be(args.PersonDetection);
        camera.DeviceType.Should().Be(DeviceTypeEnum.Camera);
    }

    [TestMethod]
    public void CreateCamera_WhenEmptyConstructor_ShouldCreateCamera()
    {
        var windowSensor = new Camera();

        windowSensor.DeviceType.Should().Be(DeviceTypeEnum.Camera);
    }
}
