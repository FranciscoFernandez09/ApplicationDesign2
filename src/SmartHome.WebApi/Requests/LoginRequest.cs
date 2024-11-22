namespace SmartHome.WebApi.Requests;

public sealed class LoginRequest(string email, string password)
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}
