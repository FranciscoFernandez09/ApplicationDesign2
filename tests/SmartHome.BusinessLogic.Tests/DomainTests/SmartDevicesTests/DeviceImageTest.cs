using FluentAssertions;
using SmartHome.BusinessLogic.Domain.SmartDevices;

namespace SmartHome.BusinessLogic.Tests.DomainTests.SmartDevicesTests;

[TestClass]
public class DeviceImageTest
{
    private const string ImageUrl = "https://www.example.com/image.jpg";
    private const bool IsMain = true;

    #region CreateDeviceImage

    #region Error

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void DeviceImage_WhenParametersAreNullOrEmpty_ShouldThrowArgumentNullException(string invalidImageUrl)
    {
        Func<DeviceImage> act = () => new DeviceImage(invalidImageUrl, IsMain);

        act.Should().ThrowExactly<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'imageUrl')");
    }

    [TestMethod]
    public void DeviceImage_WhenImageUrlIsInvalid_ShouldThrowArgumentException()
    {
        Func<DeviceImage> act = () => new DeviceImage("invalid-url", IsMain);

        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("ImageUrl must be jpg or png.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void DeviceImage_WhenValidParameters_ShouldCreateDeviceImage()
    {
        var deviceImage = new DeviceImage(ImageUrl, IsMain);

        deviceImage.Id.Should().NotBeEmpty();
        deviceImage.ImageUrl.Should().Be(ImageUrl);
        deviceImage.IsMain.Should().Be(IsMain);
    }

    #endregion

    #endregion
}
