namespace SmartHome.WebApi.Requests.Filters;

public sealed class FilterDeviceRequest()
{
    public FilterDeviceRequest(string? name, string? model, string? companyName, string? type, int? offset, int? limit)
        : this()
    {
        Name = name;
        Model = model;
        CompanyName = companyName;
        Type = type;
        Offset = offset;
        Limit = limit;
    }

    public string? Name { get; set; }
    public string? Model { get; set; }
    public string? CompanyName { get; set; }
    public string? Type { get; set; }
    public int? Offset { get; set; }
    public int? Limit { get; set; }
}
