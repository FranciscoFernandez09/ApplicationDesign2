using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Services;

public sealed class CompanyOwnerService(
    IRepository<User> userRepository,
    IRepository<Company> companyRepository,
    IRepository<SmartDevice> smartDeviceRepository,
    IRepository<Role> roleRepository,
    IModelValidatorService iModelValidatorService,
    IDeviceImporterService iDeviceImporterService)
    : ICompanyOwnerService
{
    public void CreateCompany(CreateCompanyArgs args)
    {
        if (args.Owner.HasCompany)
        {
            throw new InvalidOperationException("User already has a company.");
        }

        if (!iModelValidatorService.ValidatorIdIsValid(args.ValidatorId))
        {
            throw new InvalidOperationException("Validator not found.");
        }

        var company = new Company(args);
        companyRepository.Add(company);
        args.Owner.HasCompany = true;
        userRepository.Update(args.Owner);
    }

    public void CreateCamera(CreateCameraWithoutCompanyArgs args, User owner)
    {
        Company company = ValidateAndGetCompanyExists(owner.Id);

        ValidateUserIsCompanyOwner(owner, company);
        ValidateModel(args.Model, company.ValidatorId);

        var cameraArgs = new CreateCameraArgs(args.Name, args.Model, args.Description, company, args.Images,
            args.HasExternalUse, args.HasInternalUse, args.MotionDetection, args.PersonDetection);
        var camera = new Camera(cameraArgs);

        smartDeviceRepository.Add(camera);
    }

    public void CreateDevice(CreateSmartDeviceWithoutCompanyArgs args, User owner)
    {
        Company company = ValidateAndGetCompanyExists(owner.Id);

        ValidateUserIsCompanyOwner(owner, company);
        ValidateModel(args.Model, company.ValidatorId);

        var deviceArgs = new CreateSmartDeviceArgs(args.Name, args.Model, args.Description, company,
            args.DeviceType, args.Images);
        var device = new SmartDevice(deviceArgs);

        smartDeviceRepository.Add(device);
    }

    public void ChangeRoleToCompanyAndHomeOwner(User user)
    {
        Role role = roleRepository.Get(r => r.Id == Constant.CompanyAndHomeOwnerRoleId)!;
        user.Role = role;

        userRepository.Update(user);
    }

    public List<ShowModelValidatorsDto> GetModelValidators()
    {
        return iModelValidatorService.GetImplementations();
    }

    public void ImportDevices(Guid ddlId, Dictionary<string, string> parameters, User user)
    {
        Company company = ValidateAndGetCompanyExists(user.Id);
        List<SmartDevice> devices = iDeviceImporterService.ImportDevices(parameters, ddlId, company);

        foreach (SmartDevice device in devices)
        {
            smartDeviceRepository.Add(device);
        }
    }

    public List<ShowParamDto> GetDeviceImporterParameters(Guid ddlId)
    {
        Dictionary<string, string> dictionary = iDeviceImporterService.GetParameters(ddlId);
        var list = dictionary.Select(d => new ShowParamDto(d.Key, d.Value)).ToList();

        return list;
    }

    public List<ShowImporterDto> GetDeviceImporters()
    {
        return iDeviceImporterService.GetImporters();
    }

    public Company ValidateAndGetCompanyExists(Guid userId)
    {
        Company? company = companyRepository.Get(c => c.OwnerId == userId);
        if (company == null)
        {
            throw new InvalidOperationException("User does not have a company.");
        }

        return company;
    }

    private void ValidateModel(string model, Guid validatorId)
    {
        if (!iModelValidatorService.IsValidModel(validatorId, model))
        {
            throw new InvalidOperationException("Model is not valid.");
        }
    }

    private static void ValidateUserIsCompanyOwner(User user, Company company)
    {
        if (company.GetOwnerId() != user.Id)
        {
            throw new InvalidOperationException("User is not the owner of the company.");
        }
    }
}
