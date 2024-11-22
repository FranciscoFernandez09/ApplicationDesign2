using System.Globalization;

namespace SmartHome.WebApi.Requests.Filters;

public sealed class FilterNotificationRequest()
{
    public FilterNotificationRequest(string? deviceType, string? date, bool? isRead)
        : this()
    {
        DeviceType = deviceType;
        Date = date != null ? DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture) : null;
        IsRead = isRead;
    }

    public string? DeviceType { get; set; }
    public DateTime? Date { get; set; }
    public bool? IsRead { get; set; }
}
