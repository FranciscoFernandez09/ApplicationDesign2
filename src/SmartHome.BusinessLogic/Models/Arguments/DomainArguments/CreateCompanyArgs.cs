using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

public sealed class CreateCompanyArgs(string? name, User? owner, string? rut, string? logo, Guid? validatorId)
{
    public readonly string Logo = string.IsNullOrEmpty(logo) ? throw new ArgumentNullException(nameof(logo)) : logo;
    public readonly string Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
    public readonly User Owner = owner ?? throw new ArgumentNullException(nameof(owner));
    public readonly string Rut = string.IsNullOrEmpty(rut) ? throw new ArgumentNullException(nameof(rut)) : rut;

    public readonly Guid ValidatorId =
        validatorId == Guid.Empty || validatorId == null
            ? throw new ArgumentNullException(nameof(validatorId))
            : validatorId.Value;
}
