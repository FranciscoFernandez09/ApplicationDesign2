using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

public sealed class AddHomePermissionArgs(
    User? user,
    Guid? memberId,
    Guid? permissionId)
{
    public User User { get; set; } = user ?? throw new ArgumentNullException(nameof(user));
    public Guid MemberId { get; set; } = memberId ?? throw new ArgumentNullException(nameof(memberId));
    public Guid PermissionId { get; set; } = permissionId ?? throw new ArgumentNullException(nameof(permissionId));
}
