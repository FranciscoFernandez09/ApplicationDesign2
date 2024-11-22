namespace SmartHome.WebApi.Requests;

public sealed class ImportDevicesRequest(Dictionary<string, string> parameters)
{
    public Dictionary<string, string> Parameters { get; set; } = parameters;
}
