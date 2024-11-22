using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.EFCoreClasses;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;

public sealed class DeviceActionService(
    IRepository<HomeDevice> homeDeviceRepository,
    IRepository<Notification> notificationRepository,
    IRepository<User> userRepository,
    IRepository<HomeMemberNotification> homeMemberNotificationRepository)
    : IDeviceActionService
{
    public void PersonDetectionAction(Guid? hardwareId, User? user)
    {
        var evt = "Unknown person detected";

        HomeDevice homeDevice = ValidateAndGetHomeDevice(hardwareId);
        ValidateDeviceToPersonDetection(homeDevice);

        if (user != null)
        {
            var existUser = userRepository.Exists(u => u.Id == user.Id);
            if (existUser)
            {
                evt = $"Person {user.Name} {user.LastName} detected";
            }
        }

        CreateNotification(evt, homeDevice);
    }

    public void CameraMovementDetectionAction(Guid? hardwareId)
    {
        const string evt = "Movement detected";

        HomeDevice homeDevice = ValidateAndGetHomeDevice(hardwareId);
        ValidateDeviceToMotionDetection(homeDevice);

        CreateNotification(evt, homeDevice);
    }

    public void ChangeWindowSensorStateTo(Guid? hardwareId, bool isActive)
    {
        var evt = "Window state " + (isActive ? "opened" : "closed");
        HomeDevice homeDevice = ValidateAndGetHomeDevice(hardwareId);
        ValidatedWindowSensorConnected(homeDevice);
        if (homeDevice.IsActive == isActive)
        {
            throw new InvalidOperationException("Window sensor is already " + (isActive ? "opened" : "closed") + ".");
        }

        homeDevice.IsActive = isActive;
        homeDeviceRepository.Update(homeDevice);

        CreateNotification(evt, homeDevice);
    }

    public void ChangeSmartLampStateTo(User user, Guid? hardwareId, bool isActive)
    {
        HomeDevice homeDevice = ValidateAndGetHomeDevice(hardwareId);
        IsSmartLampAndIsConnected(homeDevice);
        Home home = homeDevice.Home;
        var isMember = home.IsMember(user);
        if (!isMember)
        {
            throw new InvalidOperationException("User is not a member of the home.");
        }

        if (homeDevice.IsActive == isActive)
        {
            throw new InvalidOperationException("Smart lamp is already turned " + (isActive ? "on" : "off") + ".");
        }

        homeDevice.IsActive = isActive;
        homeDeviceRepository.Update(homeDevice);

        var evt = "Smart lamp turned " + (isActive ? "on" : "off");
        CreateNotification(evt, homeDevice);
    }

    public void MotionSensorMovementDetection(Guid? hardwareId)
    {
        const string evt = "Movement detected";
        HomeDevice homeDevice = ValidateAndGetHomeDevice(hardwareId);
        var isMotionSensor = homeDevice.IsMotionSensor();
        if (!isMotionSensor)
        {
            throw new InvalidOperationException("Smart device is not a motion sensor.");
        }

        CreateNotification(evt, homeDevice);
    }

    private static void IsSmartLampAndIsConnected(HomeDevice homeDevice)
    {
        if (!homeDevice.IsSmartLamp())
        {
            throw new InvalidOperationException("Smart device is not a smart lamp.");
        }

        if (!homeDevice.IsConnected)
        {
            throw new InvalidOperationException("Smart lamp is not connected.");
        }
    }

    private void CreateNotification(string evt, HomeDevice homeDevice)
    {
        Home home = homeDevice.Home;
        List<HomeMember> members = home.GetMembersToNotify();

        var notification = new Notification(evt, homeDevice, []);
        notificationRepository.Add(notification);
        foreach (HomeMemberNotification homeMemberNotification in members.Select(m =>
                     new HomeMemberNotification(m, notification)))
        {
            homeMemberNotificationRepository.Add(homeMemberNotification);
        }
    }

    private HomeDevice ValidateAndGetHomeDevice(Guid? hardwareId)
    {
        ValidateGuid(hardwareId, nameof(hardwareId));

        HomeDevice? homeDevice = homeDeviceRepository.Get(hd => hd.Id == hardwareId);
        if (homeDevice == null)
        {
            throw new InvalidOperationException("Device is not added to home.");
        }

        return homeDevice;
    }

    private static void ValidateDeviceToMotionDetection(HomeDevice homeDevice)
    {
        ValidatedCameraConnected(homeDevice);
        var camera = (Camera)homeDevice.Device;

        if (!camera.MotionDetection)
        {
            throw new InvalidOperationException("Camera has not motion detection.");
        }
    }

    private static void ValidateDeviceToPersonDetection(HomeDevice homeDevice)
    {
        ValidatedCameraConnected(homeDevice);
        var camera = (Camera)homeDevice.Device;

        if (!camera.PersonDetection)
        {
            throw new InvalidOperationException("Camera has not detection person.");
        }
    }

    private static void ValidatedCameraConnected(HomeDevice homeDevice)
    {
        if (!homeDevice.IsCamera())
        {
            throw new InvalidOperationException("Smart device is not a camera.");
        }

        if (!homeDevice.IsConnected)
        {
            throw new InvalidOperationException("Camera is not connected.");
        }
    }

    private static void ValidatedWindowSensorConnected(HomeDevice homeDevice)
    {
        if (!homeDevice.IsWindowSensor())
        {
            throw new InvalidOperationException("Smart device is not a window sensor.");
        }

        if (!homeDevice.IsConnected)
        {
            throw new InvalidOperationException("Window sensor is not connected.");
        }
    }

    private static void ValidateGuid(Guid? value, string parameterName)
    {
        if (value == null || value == Guid.Empty)
        {
            throw new ArgumentNullException(parameterName);
        }
    }
}
