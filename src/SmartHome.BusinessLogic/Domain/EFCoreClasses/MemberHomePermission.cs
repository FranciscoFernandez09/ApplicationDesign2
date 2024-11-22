using SmartHome.BusinessLogic.Domain.HomeManagement;

namespace SmartHome.BusinessLogic.Domain.EFCoreClasses;

public sealed class MemberHomePermission()
{
    public MemberHomePermission(HomeMember member, HomePermission permission)
        : this()
    {
        MemberId = member.Id;
        Member = member;
        PermissionId = permission.Id;
        Permission = permission;
    }

    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid MemberId { get; init; }
    public HomeMember Member { get; init; } = null!;
    public Guid PermissionId { get; init; }
    public HomePermission Permission { get; init; } = null!;
}
