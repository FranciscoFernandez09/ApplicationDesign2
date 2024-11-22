using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Interfaces;

public interface IAdminService
{
    public void CreateAdmin(CreateUserArgs args);
    public void DeleteAdmin(Guid deleteId);
    public void CreateCompanyOwner(CreateUserArgs args);
    public List<ShowUserDto> GetUsers(FilterUserArgs args);
    public List<ShowCompanyDto> GetCompanies(FilterCompanyArgs args);
    public void ChangeRoleToAdminHomeOwner(User user);
}
