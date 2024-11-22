using System.Text.RegularExpressions;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

namespace SmartHome.BusinessLogic.Domain.SmartDevices;

public enum DeviceTypeEnum
{
    /// <summary>
    ///     A smart lamp device.
    /// </summary>
    SmartLamp,

    /// <summary>
    ///     A motion sensor device.
    /// </summary>
    MotionSensor,

    /// <summary>
    ///     A window sensor device.
    /// </summary>
    WindowSensor,

    /// <summary>
    ///     A camera device.
    /// </summary>
    Camera
}

public class SmartDevice()
{
    public SmartDevice(CreateSmartDeviceArgs args)
        : this()
    {
        if (!AreValidParameters(args.Name, args.Description, out var errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }

        Id = Guid.NewGuid();
        Name = args.Name;
        Model = args.Model;
        Description = args.Description;
        CreateOn = DateTime.Now;
        CompanyOwner = args.CompanyOwner;
        CompanyOwnerId = CompanyOwner.Id;
        Images = args.Images;
        DeviceType = ParseToEnum(args.DeviceType);
        HomeDevices = [];
    }

    public Guid Id { get; init; }
    public DeviceTypeEnum DeviceType { get; init; }
    public string Name { get; init; } = null!;
    public string Model { get; init; } = null!;
    public string Description { get; init; } = null!;
    public DateTime CreateOn { get; init; }
    public Company CompanyOwner { get; init; } = null!;
    public Guid CompanyOwnerId { get; init; }
    public List<DeviceImage> Images { get; init; } = null!;
    public List<HomeDevice> HomeDevices { get; set; } = null!;

    public DeviceImage GetMainImage()
    {
        return Images.FirstOrDefault(x => x.IsMain)!;
    }

    public string GetMainImageUrl()
    {
        return GetMainImage().ImageUrl;
    }

    private static DeviceTypeEnum ParseToEnum(string deviceType)
    {
        DeviceTypeEnum result = deviceType switch
        {
            "SmartLamp" => DeviceTypeEnum.SmartLamp,
            "MotionSensor" => DeviceTypeEnum.MotionSensor,
            "WindowSensor" => DeviceTypeEnum.WindowSensor,
            "Camera" => DeviceTypeEnum.Camera,
            _ => throw new ArgumentException("Invalid device type.")
        };

        return result;
    }

    private static bool AreValidParameters(string name, string description, out string errorMessage)
    {
        return IsValidName(name, out errorMessage) && IsValidDescription(description, out errorMessage);
    }

    private static bool IsValidName(string name, out string errorMessage)
    {
        var condition = !IsValidParameter(name);
        if (condition)
        {
            errorMessage = "Invalid name: Only letters, numbers and spaces are allowed.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsValidDescription(string description, out string errorMessage)
    {
        var condition = !IsValidParameter(description);
        if (condition)
        {
            errorMessage = "Invalid description: Only letters, numbers and spaces are allowed.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsValidParameter(string parameter)
    {
        return Regex.IsMatch(parameter, @"^[a-zA-Z0-9\s]+$");
    }
}
