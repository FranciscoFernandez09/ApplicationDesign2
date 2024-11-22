using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Interfaces;

public interface IUserService
{
    public List<ShowDeviceDto> GetDevices(FilterDeviceArgs args);

    public List<string> GetDevicesTypes();

    public void ModifyProfileImage(User user, string? profileImage);
}
