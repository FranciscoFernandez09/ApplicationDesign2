using System.Text.RegularExpressions;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

namespace SmartHome.BusinessLogic.Domain;

public sealed class User()
{
    public User(CreateUserArgs args)
        : this()
    {
        if (!AreValidParameters(args.Name, args.LastName, args.Email, args.Password, args.ProfileImage,
                out var errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }

        Id = Guid.NewGuid();
        Name = args.Name;
        LastName = args.LastName;
        Email = args.Email;
        Password = args.Password;
        Role = args.Role;
        RoleId = Role.Id;
        ProfileImage = args.ProfileImage;
        CreatedAt = args.CreatedAt;
        HasCompany = false;
        HomeMembers = [];
    }

    public Guid Id { get; init; }
    public string LastName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public Role Role { get; set; } = null!;
    public Guid RoleId { get; init; }
    public string? ProfileImage { get; set; }
    public DateTime CreatedAt { get; init; }
    public bool HasCompany { get; set; }
    public List<HomeMember> HomeMembers { get; set; } = null!;

    public bool RoleHasRequiredSystemPermission(Guid permission)
    {
        return Role.HasSystemPermission(permission);
    }

    public string GetFullName()
    {
        return $"{Name} {LastName}";
    }

    public bool IsHomeOwnerRole()
    {
        Guid validRole1 = Constant.HomeOwnerRoleId;
        Guid validRole2 = Constant.AdminHomeOwnerRoleId;
        Guid validRole3 = Constant.CompanyAndHomeOwnerRoleId;

        return Role.Id == validRole1 || Role.Id == validRole2 || Role.Id == validRole3;
    }

    public void ModifyProfileImage(string? profileImage)
    {
        if (!IsValidImage(profileImage, out var errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }

        ProfileImage = profileImage;
    }

    private static bool AreValidParameters(string name, string lastName, string email, string password, string? image,
        out string errorMessage)
    {
        return IsValidName(name, out errorMessage) && IsValidLastName(lastName, out errorMessage) &&
               IsValidEmail(email, out errorMessage) && IsValidPassword(password, out errorMessage) &&
               IsValidImage(image, out errorMessage);
    }

    private static bool IsValidName(string name, out string errorMessage)
    {
        var condition = !Regex.IsMatch(name, @"^[a-z A-Z]+$");
        if (condition)
        {
            errorMessage = "Invalid name: Only letters and spaces are allowed.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsValidLastName(string lastName, out string errorMessage)
    {
        var condition = !Regex.IsMatch(lastName, @"^[a-z A-Z]+$");
        if (condition)
        {
            errorMessage = "Invalid last name: Only letters and spaces are allowed.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsValidEmail(string email, out string errorMessage)
    {
        var condition = !Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        if (condition)
        {
            errorMessage = "Invalid email: Must have an special character '@', a domain and letters before the '@'.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsValidImage(string? image, out string errorMessage)
    {
        if (!string.IsNullOrEmpty(image))
        {
            var condition = !Regex.IsMatch(image, @".*\.(png|jpg)$");
            if (condition)
            {
                errorMessage = "Invalid image: Must be a valid image format (jpg, png).";
                return false;
            }
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsValidPassword(string password, out string errorMessage)
    {
        const int maxLengthPassword = 8;

        if (password.Length < maxLengthPassword)
        {
            errorMessage =
                "Invalid password: Must be at least 8 characters long and include numbers, special character, uppercase and lowercase letters.";
            return false;
        }

        var condition = !Regex.IsMatch(password, @"[\.#@$,%!^&*?+=_-]") ||
                        !Regex.IsMatch(password, "[a-z]") || !Regex.IsMatch(password, "[A-Z]") ||
                        !Regex.IsMatch(password, "[0-9]");

        if (condition)
        {
            errorMessage =
                "Invalid password: Must be at least 8 characters long and include numbers, special character, uppercase and lowercase letters.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    public override string ToString()
    {
        return $"Name: {Name} - LastName: {LastName} - Email: {Email} - ProfileImage: {ProfileImage}";
    }
}
