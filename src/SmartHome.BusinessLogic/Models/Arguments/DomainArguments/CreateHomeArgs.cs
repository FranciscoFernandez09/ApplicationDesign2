using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

public sealed class CreateHomeArgs(
    string? addressStreet,
    int? addressNumber,
    int? latitude,
    int? longitude,
    int? maxMembers,
    string? name,
    User? owner)
{
    public readonly int AddressNumber = addressNumber ?? throw new ArgumentNullException(nameof(addressNumber));

    public readonly string AddressStreet = string.IsNullOrEmpty(addressStreet)
        ? throw new ArgumentNullException(nameof(addressStreet))
        : addressStreet;

    public readonly int Latitude = latitude ?? throw new ArgumentNullException(nameof(latitude));
    public readonly int Longitude = longitude ?? throw new ArgumentNullException(nameof(longitude));
    public readonly int MaxMembers = maxMembers ?? throw new ArgumentNullException(nameof(maxMembers));

    public readonly string Name = string.IsNullOrEmpty(name)
        ? throw new ArgumentNullException(nameof(name))
        : name;

    public readonly User Owner = owner ?? throw new ArgumentNullException(nameof(owner));
}
