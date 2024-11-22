namespace SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;

public sealed class FilterCompanyArgs(
    string? companyName,
    string? ownerName,
    string? ownerLastName,
    int? offset,
    int? limit)
    : PaginationArgs(offset, limit)
{
    public string? CompanyName { get; set; } = companyName;
    public string? OwnerName { get; set; } = ownerName;
    public string? OwnerLastName { get; set; } = ownerLastName;
}
