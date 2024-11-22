using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Services;

public sealed class AdminService(
    IRepository<User> userRepository,
    IRepository<Role> roleRepository,
    IRepository<Company> companyRepository)
    : IAdminService
{
    private const string MessageEmailExists = "Email already exists.";

    public void CreateAdmin(CreateUserArgs args)
    {
        Role admin = roleRepository.Get(r => r.Id == Constant.AdminRoleId)!;
        args.Role = admin;

        if (ExistsEmail(args.Email))
        {
            throw new InvalidOperationException(MessageEmailExists);
        }

        var user = new User(args);
        userRepository.Add(user);
    }

    public void DeleteAdmin(Guid deleteId)
    {
        if (deleteId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(deleteId));
        }

        User? user = userRepository.Get(u => u.Id == deleteId);
        if (user is null || user.Role.Id != Constant.AdminRoleId)
        {
            throw new InvalidOperationException("Admin not exists.");
        }

        userRepository.Delete(user);
    }

    public void CreateCompanyOwner(CreateUserArgs args)
    {
        Role companyOwner = roleRepository.Get(r => r.Id == Constant.CompanyOwnerRoleId)!;
        args.Role = companyOwner;

        if (ExistsEmail(args.Email))
        {
            throw new InvalidOperationException(MessageEmailExists);
        }

        var user = new User(args);
        userRepository.Add(user);
    }

    public List<ShowUserDto> GetUsers(FilterUserArgs args)
    {
        List<User> users = userRepository.GetAll(u =>
            (string.IsNullOrEmpty(args.Name) || string.IsNullOrEmpty(args.LastName) ||
             (u.Name.Contains(args.Name) && u.LastName.Contains(args.LastName))) &&
            (string.IsNullOrEmpty(args.Role) || u.Role.Name == args.Role), args.Offset, args.Limit);

        return users.Select(MapUserToShowUserDto).ToList();
    }

    public List<ShowCompanyDto> GetCompanies(FilterCompanyArgs args)
    {
        List<Company> companies = companyRepository.GetAll(c =>
            (string.IsNullOrEmpty(args.CompanyName) || c.Name == args.CompanyName) &&
            (string.IsNullOrEmpty(args.OwnerName) || string.IsNullOrEmpty(args.OwnerLastName) ||
             (c.Owner.Name == args.OwnerName && c.Owner.LastName == args.OwnerLastName)), args.Offset, args.Limit);

        return companies.Select(MapCompanyToShowCompanyDto).ToList();
    }

    public void ChangeRoleToAdminHomeOwner(User user)
    {
        Role role = roleRepository.Get(r => r.Id == Constant.AdminHomeOwnerRoleId)!;
        user.Role = role;

        userRepository.Update(user);
    }

    private bool ExistsEmail(string email)
    {
        return userRepository.Exists(u => u.Email == email);
    }

    private static ShowUserDto MapUserToShowUserDto(User user)
    {
        return new ShowUserDto(user.Id, user.Name, user.LastName, user.Role.Name, user.CreatedAt);
    }

    private static ShowCompanyDto MapCompanyToShowCompanyDto(Company company)
    {
        var fullName = company.GetOwnerFullName();
        var email = company.GetOwnerEmail();
        return new ShowCompanyDto(company.Id, company.Name, fullName, email, company.Rut);
    }
}
