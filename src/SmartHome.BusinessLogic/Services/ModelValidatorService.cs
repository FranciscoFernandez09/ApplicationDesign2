using ModeloValidador.Abstracciones;
using SmartHome.BusinessLogic.AssemblyManagement;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Services;

public sealed class ModelValidatorService : IModelValidatorService
{
    public bool IsValidModel(Guid validatorId, string value)
    {
        try
        {
            IModeloValidador validator = GetValidatorByGuid(validatorId);
            var model = new Modelo(value);
            return validator.EsValido(model);
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool ValidatorIdIsValid(Guid validatorId)
    {
        try
        {
            GetValidatorByGuid(validatorId);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public List<ShowModelValidatorsDto> GetImplementations()
    {
        try
        {
            LoadAssembly<IModeloValidador> loadAssembly = GetLoadAssembly();
            List<string> implementations = loadAssembly.GetImplementations();

            return (from imp in implementations
                    let id = GetImplementationId(imp)
                    select new ShowModelValidatorsDto(id, imp)).ToList();
        }
        catch (Exception)
        {
            throw new AssemblyException("Error loading validators.");
        }
    }

    private static IModeloValidador GetValidatorByGuid(Guid validatorId)
    {
        try
        {
            LoadAssembly<IModeloValidador> loadAssembly = GetLoadAssembly();
            loadAssembly.GetImplementations();
            IModeloValidador validator = loadAssembly.GetImplementationByGuid(validatorId);
            return validator;
        }
        catch (Exception)
        {
            throw new AssemblyException("Error loading validators.");
        }
    }

    private static Guid GetImplementationId(string name)
    {
        LoadAssembly<IModeloValidador> loadAssembly = GetLoadAssembly();
        loadAssembly.GetImplementations();
        return loadAssembly.GetImplementationIdByName(name);
    }

    private static LoadAssembly<IModeloValidador> GetLoadAssembly()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory + Constant.ValidatorsPathAddedToCurrent;
        return new LoadAssembly<IModeloValidador>(path);
    }
}
