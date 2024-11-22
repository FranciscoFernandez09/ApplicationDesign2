using System.Text.Json.Serialization;
using JsonDevicesImporter.DTOs;

namespace JsonDevicesImporter;

public sealed class DevicesWrapper
{
    [JsonPropertyName("dispositivos")]
    public List<JsonDeviceDto> Devices { get; init; } = [];
}
