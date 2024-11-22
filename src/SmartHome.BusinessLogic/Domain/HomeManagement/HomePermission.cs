using System.Text.RegularExpressions;

namespace SmartHome.BusinessLogic.Domain.HomeManagement;

public sealed class HomePermission()
{
    public HomePermission(string name)
        : this()
    {
        Name = ValidateName(name);
    }

    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = null!;
    public List<HomeMember> HomeMembers { get; set; } = [];

    public override string ToString()
    {
        return $"Permission: {Name}";
    }

    public void AddHomeMember(HomeMember homeMember)
    {
        HomeMembers.Add(homeMember);
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
            throw new ArgumentException("Invalid home permission name: Only letters, numbers and spaces are allowed.");
        }

        return name;
    }
}
