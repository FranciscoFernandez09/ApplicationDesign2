namespace SmartHome.BusinessLogic.Models.ResponseDTOs;

public sealed class ShowNotificationDto(Guid notificationId, string @event, Guid device, bool isRead, DateTime date)
{
    public Guid NotificationId { get; init; } = notificationId;
    public string Event { get; init; } = @event;
    public Guid Device { get; init; } = device;
    public bool IsRead { get; init; } = isRead;
    public DateTime Date { get; init; } = date;
}
