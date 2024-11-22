namespace SmartHome.WebApi.Requests;

public sealed class UpdateProfileImageRequest(string? profileImage)
{
    public string? ProfileImage { get; set; } = profileImage;
}
