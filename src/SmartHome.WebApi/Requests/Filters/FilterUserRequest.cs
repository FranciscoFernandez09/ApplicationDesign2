namespace SmartHome.WebApi.Requests.Filters;

public sealed class FilterUserRequest()
{
    public FilterUserRequest(string? name, string? lastName, string? role, int? offset, int? limit)
        : this()
    {
        Name = name;
        LastName = lastName;
        Role = role;
        Offset = offset;
        Limit = limit;
    }

    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Role { get; set; }
    public int? Offset { get; set; }
    public int? Limit { get; set; }
}
