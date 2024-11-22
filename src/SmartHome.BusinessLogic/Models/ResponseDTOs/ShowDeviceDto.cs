namespace SmartHome.BusinessLogic.Models.ResponseDTOs;

public sealed class ShowDeviceDto(Guid id, string name, string model, string? mainImageUrl, string companyOwnerName)
{
    public Guid Id { get; init; } = id;
    public string Name { get; init; } = name;
    public string Model { get; init; } = model;
    public string? MainImageUrl { get; init; } = mainImageUrl;
    public string CompanyOwnerName { get; init; } = companyOwnerName;
}
