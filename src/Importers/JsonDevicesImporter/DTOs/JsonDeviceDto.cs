using System.Text.Json.Serialization;

namespace JsonDevicesImporter.DTOs;

public sealed class JsonDeviceDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("tipo")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("nombre")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("modelo")]
    public string Model { get; set; } = null!;

    [JsonPropertyName("fotos")]
    public List<JsonImageDto> Images { get; set; } = null!;

    [JsonPropertyName("person_detection")]
    public bool? PersonDetection { get; set; } = null!;

    [JsonPropertyName("movement_detection")]
    public bool? MovementDetection { get; set; } = null!;
}
