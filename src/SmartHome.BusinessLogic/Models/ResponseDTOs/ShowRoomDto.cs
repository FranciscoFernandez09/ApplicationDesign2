namespace SmartHome.BusinessLogic.Models.ResponseDTOs;

public sealed class ShowRoomDto(Guid id, string name)
{
    public Guid Id { get; init; } = id;
    public string Name { get; init; } = name;
}
