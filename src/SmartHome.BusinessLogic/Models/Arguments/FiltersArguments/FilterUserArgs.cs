namespace SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;

public sealed class FilterUserArgs(string? name, string? lastName, string? role, int? offset, int? limit)
    : PaginationArgs(offset, limit)
{
    public string? Name { get; set; } = name;
    public string? LastName { get; set; } = lastName;
    public string? Role { get; set; } = role;
}
