namespace SmartHome.WebApi.Requests;

public sealed class DeviceImageRequest(string imageUrl, bool isMain)
{
    public bool IsMain { get; set; } = isMain;
    public string ImageUrl { get; set; } = imageUrl;
}
