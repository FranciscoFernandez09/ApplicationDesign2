using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.SmartDevices;

namespace SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;

public sealed class FilterNotificationsArgs(User currentUser, string? deviceType, DateTime? date, bool? isRead)
{
    public User CurrentUser { get; set; } = currentUser;

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

    public DateTime? Date { get; set; } =
        date != null ? new DateTime(date.Value.Year, date.Value.Month, date.Value.Day) : null;

    public bool? IsRead { get; set; } = isRead;
}
