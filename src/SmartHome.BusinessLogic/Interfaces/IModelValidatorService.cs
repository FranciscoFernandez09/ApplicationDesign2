using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Interfaces;

public interface IModelValidatorService
{
    public bool IsValidModel(Guid validatorId, string value);
    public bool ValidatorIdIsValid(Guid validatorId);
    public List<ShowModelValidatorsDto> GetImplementations();
}
