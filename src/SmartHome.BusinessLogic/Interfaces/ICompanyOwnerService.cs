using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Models.Arguments;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Interfaces;

public interface ICompanyOwnerService
{
    void CreateCompany(CreateCompanyArgs args);
    void CreateCamera(CreateCameraWithoutCompanyArgs args, User owner);
    void CreateDevice(CreateSmartDeviceWithoutCompanyArgs args, User owner);
    void ChangeRoleToCompanyAndHomeOwner(User user);
    Company ValidateAndGetCompanyExists(Guid userId);
    List<ShowModelValidatorsDto> GetModelValidators();
    void ImportDevices(Guid ddlId, Dictionary<string, string> parameters, User user);
    List<ShowImporterDto> GetDeviceImporters();
    public List<ShowParamDto> GetDeviceImporterParameters(Guid ddlId);
}
