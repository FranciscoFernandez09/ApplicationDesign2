using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Interfaces;

public interface IDeviceImporterService
{
    List<SmartDevice> ImportDevices(Dictionary<string, string> parameters, Guid ddlId, Company company);
    List<ShowImporterDto> GetImporters();
    Dictionary<string, string> GetParameters(Guid importerId);
}
