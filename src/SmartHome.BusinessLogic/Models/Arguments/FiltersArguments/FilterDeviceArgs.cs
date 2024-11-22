using SmartHome.BusinessLogic.Domain.SmartDevices;

namespace SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;

public sealed class FilterDeviceArgs(
    string? name,
    string? model,
    string? companyName,
    string? deviceType,
    int? offset,
    int? limit)
    : PaginationArgs(offset, limit)
{
    public string? Name { get; set; } = name;
    public string? Model { get; set; } = model;
    public string? CompanyName { get; set; } = companyName;

    public DeviceTypeEnum? DeviceType { get; set; } = deviceType != null
        ? deviceType switch
        {
            "SmartLamp" => DeviceTypeEnum.SmartLamp,
            "MotionSensor" => DeviceTypeEnum.MotionSensor,
            "WindowSensor" => DeviceTypeEnum.WindowSensor,
            "Camera" => DeviceTypeEnum.Camera,
            _ => null
        }
        : null;
}
