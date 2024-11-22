using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.SmartDevices;

namespace SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

public class CreateSmartDeviceArgs(
    string? name,
    string? model,
    string? description,
    Company? companyOwner,
    string deviceType,
    List<DeviceImage>? images)
{
    public readonly Company CompanyOwner = companyOwner ?? throw new ArgumentNullException(nameof(companyOwner));

    public readonly string Description = string.IsNullOrEmpty(description)
        ? throw new ArgumentNullException(nameof(description))
        : description;

    public readonly string DeviceType = string.IsNullOrEmpty(deviceType)
        ? throw new ArgumentNullException(nameof(deviceType))
        : deviceType;

    public readonly List<DeviceImage> Images =
        images == null ? throw new ArgumentNullException(nameof(companyOwner)) : ImagesAreValid(images);

    public readonly string Model = string.IsNullOrEmpty(model)
        ? throw new ArgumentNullException(nameof(model))
        : model;

    public readonly string Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;

    private static List<DeviceImage> ImagesAreValid(List<DeviceImage> images)
    {
        if (images.All(i => !i.IsMain))
        {
            throw new ArgumentException("Main image is required.");
        }

        if (images.Count(i => i.IsMain) > 1)
        {
            throw new ArgumentException("Only one main image is allowed");
        }

        return images;
    }
}
