namespace SmartHome.BusinessLogic.Models.ResponseDTOs;

public sealed class ShowModelValidatorsDto(Guid id, string name)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
}
