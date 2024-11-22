using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Interfaces.HomeManagement;
using SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Services.HomeManagement;

public sealed class MemberService(
    IRepository<HomeMember> homeMemberRepository,
    IRepository<Notification> notificationRepository)
    : IMemberService
{
    public void ChangeMemberNotificationStateTo(User currentUser, Guid? memberId, bool state)
    {
        ThrowExceptionWhenParameterIsNullOrEmpty(memberId, nameof(memberId));

        HomeMember? homeMember = homeMemberRepository.Get(hm => hm.Id == memberId);

        if (homeMember == null)
        {
            throw new InvalidOperationException("Member not found in the home.");
        }

        Home home = homeMember.Home;

        if (!home.IsOwner(currentUser))
        {
            throw new UnauthorizedAccessException(
                "User does not have permission to modify member notification privileges.");
        }

        homeMember.ShouldNotify = state;
        homeMemberRepository.Update(homeMember);
    }

    public List<ShowNotificationDto> GetNotifications(FilterNotificationsArgs args)
    {
        List<Notification> notifications = notificationRepository.GetAll(n =>
            n.Members.Any(m => m.UserId == args.CurrentUser.Id) &&
            (args.Date == null || (n.EventDate.Day == args.Date!.Value.Day &&
                                   n.EventDate.Month == args.Date.Value.Month &&
                                   n.EventDate.Year == args.Date.Value.Year)) &&
            (args.DeviceType == null || n.HomeDevice.Device.DeviceType == args.DeviceType) &&
            (args.IsRead == null || n.IsRead == args.IsRead));

        var result = notifications.Select(n =>
        {
            var isRead = n.IsRead;
            n.IsRead = true;
            notificationRepository.Update(n);
            return new ShowNotificationDto(n.Id, n.Event, n.HomeDevice.Id, isRead, n.EventDate);
        }).ToList();

        return result;
    }

    private static void ThrowExceptionWhenParameterIsNullOrEmpty(Guid? parameter, string parameterName)
    {
        if (parameter == null || parameter == Guid.Empty)
        {
            throw new ArgumentNullException(parameterName);
        }
    }
}
