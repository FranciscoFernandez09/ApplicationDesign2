namespace SmartHome.WebApi.Requests;

public sealed class AddMemberRequest(string? memberEmail)
{
    public string? MemberEmail { get; set; } = memberEmail;
}
