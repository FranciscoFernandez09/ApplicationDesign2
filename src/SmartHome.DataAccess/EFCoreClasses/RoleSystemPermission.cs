using SmartHome.BusinessLogic.Domain;

namespace SmartHome.DataAccess.EFCoreClasses;

public sealed class RoleSystemPermission()
{
    public RoleSystemPermission(Role role, SystemPermission permission)
        : this()
    {
        RoleId = role.Id;
        Role = role;
        PermissionId = permission.Id;
        Permission = permission;
    }

    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid RoleId { get; init; }
    public Role Role { get; init; } = null!;
    public Guid PermissionId { get; init; }
    public SystemPermission Permission { get; init; } = null!;
}
