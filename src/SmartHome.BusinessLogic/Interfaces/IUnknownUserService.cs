using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

namespace SmartHome.BusinessLogic.Interfaces;

public interface IUnknownUserService
{
    public void CreateHomeOwner(CreateUserArgs args);
}
