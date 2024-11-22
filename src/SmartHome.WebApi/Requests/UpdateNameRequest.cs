namespace SmartHome.WebApi.Requests;

public sealed class UpdateNameRequest(string? name)
{
    public string? Name { get; set; } = name;
}
