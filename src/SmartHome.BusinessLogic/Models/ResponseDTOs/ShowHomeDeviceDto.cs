namespace SmartHome.BusinessLogic.Models.ResponseDTOs;

public sealed class ShowHomeDeviceDto(
    Guid homeDeviceId,
    string name,
    string type,
    bool isActive,
    string model,
    string mainImage,
    bool isConnected)
{
    public Guid HomeDeviceId { get; init; } = homeDeviceId;
    public string Name { get; init; } = name;
    public string Type { get; init; } = type;
    public bool IsActive { get; init; } = isActive;
    public string Model { get; init; } = model;
    public string MainImage { get; init; } = mainImage;
    public bool IsConnected { get; init; } = isConnected;
}
