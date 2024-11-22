using System.Text.RegularExpressions;

namespace SmartHome.BusinessLogic.Domain.SmartDevices;

public sealed class DeviceImage(string imageUrl, bool isMain)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public bool IsMain { get; init; } = isMain;

    public string ImageUrl { get; init; } =
        string.IsNullOrEmpty(imageUrl)
            ? throw new ArgumentNullException(nameof(imageUrl))
            : !Regex.IsMatch(imageUrl, @".*\.(png|jpg|webp)$")
                ? throw new ArgumentException("ImageUrl must be jpg or png.")
                : imageUrl;
}
