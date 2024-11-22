namespace SmartHome.BusinessLogic.Models.ResponseDTOs;

public sealed class ShowHomeDto(Guid homeId, string name)
{
    public Guid HomeId { get; init; } = homeId;
    public string Name { get; init; } = name;
}
