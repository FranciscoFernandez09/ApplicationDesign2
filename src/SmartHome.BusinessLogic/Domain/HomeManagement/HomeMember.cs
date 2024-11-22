namespace SmartHome.BusinessLogic.Domain.HomeManagement;

public sealed class HomeMember()
{
    public HomeMember(Home home, User user)
        : this()
    {
        HomeId = home.Id;
        Home = home;
        UserId = user.Id;
        User = user;
    }

    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid HomeId { get; init; }
    public Home Home { get; init; } = null!;
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
    public bool ShouldNotify { get; set; }
    public List<HomePermission> Permissions { get; set; } = [];
    public List<Notification> Notifications { get; set; } = [];

    public bool HasHomePermission(Guid permission)
    {
        return Permissions.Any(p => p.Id == permission);
    }

    public void AddHomePermission(HomePermission permission)
    {
        permission.AddHomeMember(this);
        Permissions.Add(permission);
    }

    public string[] GetUserData()
    {
        return [User.GetFullName(), User.Email, User.ProfileImage!, ConcatenatePermissionsName(Permissions)];
    }

    public void OwnerMemberSettings()
    {
        ShouldNotify = true;
    }

    private static string ConcatenatePermissionsName(List<HomePermission> permissions)
    {
        return string.Join(", ", permissions.Select(p => p.Name));
    }
}
