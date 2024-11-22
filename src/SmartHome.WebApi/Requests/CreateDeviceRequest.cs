namespace SmartHome.WebApi.Requests;

public class CreateDeviceRequest(
    string? name,
    string? model,
    string? description,
    List<DeviceImageRequest>? images)
{
    public string? Name { get; set; } = name;
    public string? Model { get; set; } = model;
    public string? Description { get; set; } = description;
    public List<DeviceImageRequest>? Images { get; set; } = images;
}
