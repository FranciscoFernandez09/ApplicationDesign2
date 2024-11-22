using System.Text.RegularExpressions;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

namespace SmartHome.BusinessLogic.Domain;

public sealed class Company()
{
    public Company(CreateCompanyArgs args)
        : this()
    {
        if (!AreValidParameters(args.Name, args.Rut, args.Logo, out var errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }

        Name = args.Name;
        Owner = args.Owner;
        OwnerId = Owner.Id;
        Rut = args.Rut;
        Logo = args.Logo;
        ValidatorId = args.ValidatorId;
    }

    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = null!;
    public User Owner { get; init; } = null!;
    public Guid OwnerId { get; init; }
    public string Rut { get; init; } = null!;
    public string Logo { get; init; } = null!;
    public Guid ValidatorId { get; init; }

    public Guid GetOwnerId()
    {
        return OwnerId;
    }

    public string GetOwnerFullName()
    {
        return Owner.GetFullName();
    }

    public string GetOwnerEmail()
    {
        return Owner.Email;
    }

    private static bool AreValidParameters(string name, string rut, string logo, out string errorMessage)
    {
        return IsValidName(name, out errorMessage) &&
               IsValidRut(rut, out errorMessage) &&
               IsValidLogo(logo, out errorMessage);
    }

    private static bool IsValidName(string name, out string errorMessage)
    {
        var condition = !IsValidParameter(name);
        if (condition)
        {
            errorMessage = "Invalid company name: Only letters, numbers and spaces are allowed.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsValidRut(string rut, out string errorMessage)
    {
        var condition = !Regex.IsMatch(rut, @"^\d{10}-\d{1}$");
        if (condition)
        {
            errorMessage = "Invalid RUT: Format is 10 numbers - 1 number (XXXXXXXXXX-X)";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsValidLogo(string logo, out string errorMessage)
    {
        var condition = !Regex.IsMatch(logo, @"^[\w-]+\.(png|jpg)$");
        if (condition)
        {
            errorMessage = "Invalid logo: Format should be <image-name>.<png/jpg>";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsValidParameter(string parameter)
    {
        return Regex.IsMatch(parameter, @"^[a-zA-Z0-9\s]+$");
    }
}
