namespace SmartHome.WebApi.Requests.Filters;

public sealed class FilterCompanyRequest()
{
    public FilterCompanyRequest(string? companyName, string? ownerName, string? ownerLastName, int? offset, int? limit)
        : this()
    {
        CompanyName = companyName;
        OwnerName = ownerName;
        OwnerLastName = ownerLastName;
        Offset = offset;
        Limit = limit;
    }

    public string? CompanyName { get; set; }
    public string? OwnerName { get; set; }
    public string? OwnerLastName { get; set; }
    public int? Offset { get; set; }
    public int? Limit { get; set; }
}
