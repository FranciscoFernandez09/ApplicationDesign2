namespace SmartHome.BusinessLogic.Models.ResponseDTOs;

public sealed class ShowHomePermissionDto(Guid id, string name)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
}
