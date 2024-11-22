namespace SmartHome.WebApi.Requests;

public sealed class CreateUserRequest(
    string? name,
    string? lastName,
    string? email,
    string? password,
    string? profileImage)
{
    public string? Name { get; set; } = name;
    public string? LastName { get; set; } = lastName;
    public string? Email { get; set; } = email;
    public string? Password { get; set; } = password;
    public string? ProfileImage { get; set; } = profileImage;
}
