namespace SmartHome.BusinessLogic.Models.ResponseDTOs;

public sealed class ShowHomeMemberDto(
    Guid homeMemberId,
    string fullName,
    string email,
    string profileImage,
    bool shouldNotify,
    string permissions)
{
    public Guid HomeMemberId { get; init; } = homeMemberId;
    public string FullName { get; init; } = fullName;
    public string Email { get; init; } = email;
    public string ProfileImage { get; init; } = profileImage;
    public bool ShouldNotify { get; set; } = shouldNotify;
    public string Permissions { get; init; } = permissions;
}
