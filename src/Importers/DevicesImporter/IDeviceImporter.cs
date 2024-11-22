using Importer.DTOs;

namespace Importer;

public interface IDeviceImporter
{
    Guid Id { get; }
    List<DeviceImporterDto> ImportDevices(Dictionary<string, string> parameters);
    Dictionary<string, string> GetParameters();
}
