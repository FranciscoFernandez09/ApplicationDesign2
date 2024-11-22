namespace SmartHome.WebApi.Requests;

public sealed class CreateHomeRequest(
    string? name,
    string? addressStreet,
    int? addressNumber,
    int? latitude,
    int? longitude,
    int? maxMembers)
{
    public string? Name { get; set; } = name;
    public string? AddressStreet { get; set; } = addressStreet;
    public int? AddressNumber { get; set; } = addressNumber;
    public int? Latitude { get; set; } = latitude;
    public int? Longitude { get; set; } = longitude;
    public int? MaxMembers { get; set; } = maxMembers;
}
