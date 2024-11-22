using SmartHome.BusinessLogic.Domain.HomeManagement;

namespace SmartHome.BusinessLogic.Domain;

public sealed class Notification()
{
    public Notification(string evt, HomeDevice homeDevice, List<HomeMember> members)
        : this()
    {
        homeDevice = homeDevice ?? throw new ArgumentNullException(nameof(homeDevice));
        members = members ?? throw new ArgumentNullException(nameof(members));
        evt = string.IsNullOrEmpty(evt) ? throw new ArgumentNullException(nameof(evt)) : evt;

        Event = evt;
        HomeDevice = homeDevice;
        HomeDeviceId = homeDevice.Id;
        Members = members;
    }

    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime EventDate { get; init; } = DateTimeProvider.Now;
    public string Event { get; init; } = null!;
    public bool IsRead { get; set; }
    public HomeDevice HomeDevice { get; init; } = null!;
    public Guid HomeDeviceId { get; init; }
    public List<HomeMember> Members { get; init; } = null!;
}
