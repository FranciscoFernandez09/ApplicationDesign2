using System.Text.RegularExpressions;

namespace SmartHome.BusinessLogic.Domain;

public sealed class SystemPermission()
{
    public SystemPermission(string name)
        : this()
    {
        Name = ValidateName(name);
    }

    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = null!;
    public List<Role> Roles { get; set; } = [];

    public void AddRole(Role role)
    {
        Roles.Add(role);
    }

    private static string ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        var condition = !Regex.IsMatch(name, @"^[a-zA-Z0-9\s]+$");
        if (condition)
        {
            throw new ArgumentException(
                "Invalid system permission name: Only letters, numbers and spaces are allowed.");
        }

        return name;
    }
}
