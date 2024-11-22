namespace SmartHome.BusinessLogic.Domain.HomeManagement;

public sealed class Room()
{
    public Room(string name, Home home)
        : this()
    {
        Name = name;
        Home = home;
        Devices = [];
    }

    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public Home Home { get; set; } = null!;
    public List<HomeDevice> Devices { get; set; } = null!;

    public Guid GetHomeId()
    {
        return Home.Id;
    }
}
