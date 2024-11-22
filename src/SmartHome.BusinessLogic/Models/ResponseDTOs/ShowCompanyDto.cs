namespace SmartHome.BusinessLogic.Models.ResponseDTOs;

public sealed class ShowCompanyDto(Guid id, string name, string ownerFullName, string ownerEmail, string rut)
{
    public Guid Id { get; init; } = id;
    public string Name { get; init; } = name;
    public string OwnerFullName { get; init; } = ownerFullName;
    public string OwnerEmail { get; init; } = ownerEmail;
    public string Rut { get; init; } = rut;
}
