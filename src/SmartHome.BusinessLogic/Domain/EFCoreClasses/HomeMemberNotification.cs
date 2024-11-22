using SmartHome.BusinessLogic.Domain.HomeManagement;

namespace SmartHome.BusinessLogic.Domain.EFCoreClasses;

public sealed class HomeMemberNotification()
{
    public HomeMemberNotification(HomeMember homeMember, Notification notification)
        : this()
    {
        HomeMember = homeMember;
        HomeMemberId = homeMember.Id;
        Notification = notification;
        NotificationId = notification.Id;
    }

    public Guid Id { get; init; } = Guid.NewGuid();
    public HomeMember HomeMember { get; init; } = null!;
    public Guid HomeMemberId { get; init; }
    public Notification Notification { get; init; } = null!;
    public Guid NotificationId { get; init; }
}
