namespace SmartHome.BusinessLogic.Models.ResponseDTOs;

public sealed class SessionDto(Guid token, Guid role, string name)
{
    public Guid Token { get; init; } = token;
    public Guid Role { get; init; } = role;
    public string Name { get; init; } = name;
}
