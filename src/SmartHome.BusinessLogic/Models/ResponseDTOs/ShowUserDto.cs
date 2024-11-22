namespace SmartHome.BusinessLogic.Models.ResponseDTOs;

public sealed class ShowUserDto(Guid id, string name, string lastName, string role, DateTime createdAt)
{
    public Guid Id { get; init; } = id;
    public string Name { get; init; } = name;
    public string LastName { get; init; } = lastName;
    public string Role { get; init; } = role;
    public DateTime CreatedAt { get; init; } = createdAt;
}
