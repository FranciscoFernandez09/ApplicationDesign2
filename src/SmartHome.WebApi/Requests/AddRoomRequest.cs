namespace SmartHome.WebApi.Requests;

public sealed class AddRoomRequest(string? name)
{
    public string? Name { get; set; } = name;
}
