using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

public sealed class CreateUserArgs(
    string? name,
    string? lastName,
    string? email,
    string? password,
    string? profileImage)
{
    public readonly DateTime CreatedAt = DateTimeProvider.Now;

    public readonly string Email = string.IsNullOrEmpty(email) ? throw new ArgumentNullException(nameof(email)) : email;

    public readonly string LastName =
        string.IsNullOrEmpty(lastName) ? throw new ArgumentNullException(nameof(lastName)) : lastName;

    public readonly string Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;

    public readonly string Password =
        string.IsNullOrEmpty(password) ? throw new ArgumentNullException(nameof(password)) : password;

    public readonly string? ProfileImage = profileImage;
    public Role Role { get; set; } = null!;
}
