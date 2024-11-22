using System.Text.RegularExpressions;

namespace SmartHome.BusinessLogic.Domain;

public sealed class Role()
{
    public Role(string name)
        : this()
    {
        Name = ValidateName(name);
    }

    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = null!;
    public List<SystemPermission> Permissions { get; set; } = [];

    public void AddPermission(SystemPermission permission)
    {
        permission.AddRole(this);
        Permissions.Add(permission);
    }

    public bool HasSystemPermission(Guid permission)
    {
        return Permissions.Any(p => p.Id == permission);
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
            throw new ArgumentException("Invalid role name: Only letters, numbers and spaces are allowed.");
        }

        return name;
    }
}
