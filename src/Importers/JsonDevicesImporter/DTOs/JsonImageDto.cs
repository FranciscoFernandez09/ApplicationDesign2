using System.Text.Json.Serialization;

namespace JsonDevicesImporter.DTOs;

public sealed class JsonImageDto
{
    [JsonPropertyName("path")]
    public string Path { get; set; } = null!;

    [JsonPropertyName("es_principal")]
    public bool IsMain { get; set; }
}
