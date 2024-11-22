namespace SmartHome.WebApi.Requests;

public sealed class AddHomePermissionRequest(Guid? permissionId)
{
    public Guid? PermissionId { get; set; } = permissionId;
}
