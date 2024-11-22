namespace SmartHome.WebApi.Requests;

public sealed class CreateCompanyRequest()
{
    public CreateCompanyRequest(string? name, string? rut, string? logo, Guid? validatorId)
        : this()
    {
        Name = name;
        Rut = rut;
        Logo = logo;
        ValidatorId = validatorId;
    }

    public string? Name { get; set; }
    public string? Rut { get; set; }
    public string? Logo { get; set; }
    public Guid? ValidatorId { get; set; }
}
