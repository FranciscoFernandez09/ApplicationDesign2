using System.Runtime.InteropServices;
using System.Text.Json;
using Importer;
using Importer.DTOs;
using JsonDevicesImporter.DTOs;

namespace JsonDevicesImporter;

[Guid("EBA4BE04-265D-4882-9EE0-141A8E318DCA")]
public sealed class JsonImporter : IDeviceImporter
{
    public Guid Id => new("EBA4BE04-265D-4882-9EE0-141A8E318DCA");

    public List<DeviceImporterDto> ImportDevices(Dictionary<string, string> parameters)
    {
        var fileName = parameters["path"];

        if (!File.Exists(fileName))
        {
            throw new FileNotFoundException($"The file at path {fileName} does not exist.");
        }

        var json = File.ReadAllText(fileName);

        if (string.IsNullOrWhiteSpace(json))
        {
            throw new FileNotFoundException("The file path is invalid or the file is empty.");
        }

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        DevicesWrapper? devicesWrapper;
        try
        {
            devicesWrapper = JsonSerializer.Deserialize<DevicesWrapper>(json, options);
        }
        catch (JsonException ex)
        {
            throw new Exception($"Error deserializing JSON: {ex.Message}", ex);
        }

        if (devicesWrapper?.Devices == null)
        {
            throw new Exception("Invalid json format");
        }

        return DeviceDtoMapper(devicesWrapper.Devices);
    }

    public Dictionary<string, string> GetParameters()
    {
        return new Dictionary<string, string> { { "path", "string" } };
    }

    private static List<DeviceImporterDto> DeviceDtoMapper(List<JsonDeviceDto> devices)
    {
        return devices.Select(d => new DeviceImporterDto(d.Id, d.Type, d.Name, d.Model,
            d.Images.Select(f => new ImageImporterDto(f.Path, f.IsMain)).ToList(), d.PersonDetection,
            d.MovementDetection)).ToList();
    }
}
