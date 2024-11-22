using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.HomeManagement;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Interfaces.HomeManagement;

namespace SmartHome.BusinessLogic.Services.HomeManagement;

public sealed class HomeDeviceService(
    IRepository<HomeDevice> homeDeviceRepository,
    IRepository<HomeMember> homeMemberRepository)
    : IHomeDeviceService
{
    public void ConnectDevice(User currentUser, Guid? hardwareId)
    {
        ThrowExceptionWhenParameterIsNullOrEmpty(hardwareId, nameof(hardwareId));

        HomeDevice? homeDevice = homeDeviceRepository.Get(hd => hd.Id == hardwareId);
        if (homeDevice == null)
        {
            throw new InvalidOperationException("Device is not added to home.");
        }

        if (homeDevice.IsConnected)
        {
            throw new InvalidOperationException("Device is already connected.");
        }

        Home home = homeDevice.Home;

        if (!home.IsOwner(currentUser))
        {
            throw new UnauthorizedAccessException("User does not is owner of the home.");
        }

        homeDevice.IsConnected = true;
        homeDeviceRepository.Update(homeDevice);
    }

    public void DisconnectDevice(User currentUser, Guid? hardwareId)
    {
        ThrowExceptionWhenParameterIsNullOrEmpty(hardwareId, nameof(hardwareId));

        HomeDevice? homeDevice = homeDeviceRepository.Get(hd => hd.Id == hardwareId);
        if (homeDevice == null)
        {
            throw new InvalidOperationException("Device is not added to home.");
        }

        if (!homeDevice.IsConnected)
        {
            throw new InvalidOperationException("Device is already disconnected.");
        }

        Home home = homeDevice.Home;

        if (!home.IsOwner(currentUser))
        {
            throw new UnauthorizedAccessException("User does not is owner of the home.");
        }

        homeDevice.IsConnected = false;
        homeDeviceRepository.Update(homeDevice);
    }

    public void ModifyHomeDeviceName(User currentUser, Guid? hardwareId, string? name)
    {
        ThrowExceptionWhenParameterIsNullOrEmpty(hardwareId, nameof(hardwareId));
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        HomeDevice? homeDevice = homeDeviceRepository.Get(hd => hd.Id == hardwareId);
        if (homeDevice == null)
        {
            throw new InvalidOperationException("Device is not added to home.");
        }

        ValidateMemberToModifyDeviceName(currentUser, homeDevice);

        homeDevice.Name = name;
        homeDeviceRepository.Update(homeDevice);
    }

    private void ValidateMemberToModifyDeviceName(User currentUser, HomeDevice homeDevice)
    {
        HomeMember? homeMember =
            homeMemberRepository.Get(hm => hm.UserId == currentUser.Id && hm.HomeId == homeDevice.HomeId);
        if (homeMember == null)
        {
            throw new UnauthorizedAccessException("User does not is member of the home.");
        }

        Home home = homeDevice.Home;
        if (!home.IsOwner(currentUser) && !homeMember.HasHomePermission(Constant.ModifyHomeDeviceNameId))
        {
            throw new UnauthorizedAccessException("User does not have permission to modify device name.");
        }
    }

    private static void ThrowExceptionWhenParameterIsNullOrEmpty(Guid? parameter, string parameterName)
    {
        if (parameter == null || parameter == Guid.Empty)
        {
            throw new ArgumentNullException(parameterName);
        }
    }
}
