using SmartHome.BusinessLogic.Domain.SmartDevices;

namespace SmartHome.BusinessLogic.Domain.HomeManagement;

public sealed class HomeDevice()
{
    public HomeDevice(Home home, SmartDevice device)
        : this()
    {
        HomeId = home.Id;
        Home = home;
        DeviceId = device.Id;
        Device = device;
        IsConnected = true;
        IsActive = false;
    }

    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid HomeId { get; init; }
    public Home Home { get; init; } = null!;
    public Guid DeviceId { get; init; }
    public SmartDevice Device { get; init; } = null!;
    public bool IsConnected { get; set; }
    public string? Name { get; set; }
    public bool IsActive { get; set; }
    public Room? Room { get; set; }

    public string[] GetNameAndMainImage()
    {
        return [Device.Name, Device.GetMainImageUrl()];
    }

    public string GetDeviceType()
    {
        return Device.DeviceType.ToString();
    }

    public bool IsCamera()
    {
        return Device.DeviceType == DeviceTypeEnum.Camera;
    }

    public bool IsWindowSensor()
    {
        return Device.DeviceType == DeviceTypeEnum.WindowSensor;
    }

    public bool IsSmartLamp()
    {
        return Device.DeviceType == DeviceTypeEnum.SmartLamp;
    }

    public bool IsMotionSensor()
    {
        return Device.DeviceType == DeviceTypeEnum.MotionSensor;
    }

    public string GetDeviceModel()
    {
        return Device.Model;
    }

    public Guid GetHomeId()
    {
        return HomeId;
    }

    public Guid GetRoomId()
    {
        return Room?.Id ?? Guid.Empty;
    }
}
