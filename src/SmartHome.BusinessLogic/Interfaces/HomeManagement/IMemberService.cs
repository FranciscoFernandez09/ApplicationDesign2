using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Interfaces.HomeManagement;

public interface IMemberService
{
    public void ChangeMemberNotificationStateTo(User currentUser, Guid? memberId, bool state);
    public List<ShowNotificationDto> GetNotifications(FilterNotificationsArgs args);
}
