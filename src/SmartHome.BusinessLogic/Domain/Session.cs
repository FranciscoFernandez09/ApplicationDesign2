namespace SmartHome.BusinessLogic.Domain;

public sealed record Session()
{
    public Session(User user)
        : this()
    {
        UserId = user.Id;
        User = user;
    }

    public Guid SessionId { get; init; } = Guid.NewGuid();
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
}
