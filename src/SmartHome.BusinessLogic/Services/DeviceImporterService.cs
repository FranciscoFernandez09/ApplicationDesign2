using System.Reflection;
using Importer;
using Importer.DTOs;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Services;

public sealed class DeviceImporterService : IDeviceImporterService
{
    public List<SmartDevice> ImportDevices(Dictionary<string, string> parameters, Guid importerId, Company company)
    {
        try
        {
            IDeviceImporter importer = LoadImporter(importerId);
            List<DeviceImporterDto> importDevices = importer.ImportDevices(parameters);
            List<SmartDevice> devices = JsonDtoMapperToSmartDevice(importDevices, company);

            return devices;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(e.Message);
        }
    }

    public Dictionary<string, string> GetParameters(Guid importerId)
    {
        IDeviceImporter importer = LoadImporter(importerId);
        return importer.GetParameters();
    }

    public List<ShowImporterDto> GetImporters()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory + Constant.ImportersPathAddedToCurrent;
        var dllFiles = Directory.GetFiles(path, "*.dll");
        var importers = new List<ShowImporterDto>();

        foreach (var dll in dllFiles)
        {
            var assembly = Assembly.LoadFrom(dll);
            IEnumerable<Type> importerTypes = assembly.GetTypes().Where(type =>
                typeof(IDeviceImporter).IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

            importers.AddRange(importerTypes
                .Select(importerType => (IDeviceImporter)Activator.CreateInstance(importerType)!)
                .Select(importer => new ShowImporterDto(importer.Id, Path.GetFileName(dll))));
        }

        return importers;
    }

    private static IDeviceImporter LoadImporter(Guid ddlId)
    {
        var path = AppDomain.CurrentDomain.BaseDirectory + Constant.ImportersPathAddedToCurrent;
        var dllFiles = Directory.GetFiles(path, "*.dll");

        foreach (var dll in dllFiles)
        {
            var assembly = Assembly.LoadFrom(dll);
            Type? importerType = assembly.GetTypes().FirstOrDefault(type =>
                typeof(IDeviceImporter).IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

            if (importerType == null)
            {
                continue;
            }

            var importer = (IDeviceImporter)Activator.CreateInstance(importerType)!;
            if (importer.Id == ddlId)
            {
                return importer;
            }
        }

        throw new InvalidOperationException("Importer type not found.");
    }

    private static List<SmartDevice> JsonDtoMapperToSmartDevice(List<DeviceImporterDto> importDevices, Company company)
    {
        var devices = new List<SmartDevice>();
        foreach (DeviceImporterDto dtoDeviceImporter in importDevices)
        {
            var images = dtoDeviceImporter.Images.Select(image => new DeviceImage(image.Url, image.IsMain)).ToList();

            const string description = "Imported";

            var deviceType = JsonDeviceTypeMapper(dtoDeviceImporter.Type);

            if (deviceType.Equals("Camera"))
            {
                var camera = new Camera(new CreateCameraArgs(dtoDeviceImporter.Name, dtoDeviceImporter.Model,
                    description, company, images, false, false, dtoDeviceImporter.MovementDetection,
                    dtoDeviceImporter.PersonDetection));
                devices.Add(camera);
            }
            else
            {
                var device = new SmartDevice(new CreateSmartDeviceArgs(dtoDeviceImporter.Name,
                    dtoDeviceImporter.Model,
                    description, company, deviceType, images));
                devices.Add(device);
            }
        }

        return devices;
    }

    private static string JsonDeviceTypeMapper(string deviceType)
    {
        return deviceType switch
        {
            "sensor-open-close" => "WindowSensor",
            "sensor-movement" => "MotionSensor",
            "camera" => "Camera",
            _ => throw new InvalidOperationException("Invalid device type.")
        };
    }
}
