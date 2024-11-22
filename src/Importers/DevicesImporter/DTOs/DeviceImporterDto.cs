namespace Importer.DTOs;

public sealed class DeviceImporterDto(
    Guid id,
    string type,
    string name,
    string model,
    List<ImageImporterDto> images,
    bool? personDetection,
    bool? movementDetection)
{
    public Guid Id { get; init; } = id;
    public string Type { get; init; } = type;
    public string Name { get; init; } = name;
    public string Model { get; init; } = model;
    public bool? PersonDetection { get; init; } = personDetection;
    public bool? MovementDetection { get; init; } = movementDetection;
    public List<ImageImporterDto> Images { get; init; } = images;
}
