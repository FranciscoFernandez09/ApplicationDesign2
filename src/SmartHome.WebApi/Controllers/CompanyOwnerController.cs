using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Domain.SmartDevices;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.Arguments;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;

namespace SmartHome.WebApi.Controllers;

[ApiController]
[AuthenticationFilter]
public sealed class CompanyOwnerController(ICompanyOwnerService companyOwnerService) : SmartHomeControllerBase
{
    [HttpPost]
    [Route("companies")]
    [AuthorizationFilter(Constant.CreateCompany)]
    public IActionResult CreateCompany([FromBody] CreateCompanyRequest request)
    {
        User user = GetLoggedUser();
        var args = new CreateCompanyArgs(request.Name, user, request.Rut, request.Logo, request.ValidatorId);
        companyOwnerService.CreateCompany(args);

        return Ok("Company created successfully.");
    }

    [HttpPost]
    [AuthorizationFilter(Constant.CreateDevice)]
    [Route("cameras")]
    public IActionResult CreateCamera([FromBody] CreateCameraRequest request)
    {
        User user = GetLoggedUser();
        List<DeviceImage> images = GetImages(request.Images);
        var args = new CreateCameraWithoutCompanyArgs(request.Name, request.Model, request.Description, images,
            request.HasExternalUse, request.HasInternalUse, request.MotionDetection,
            request.PersonDetection);

        companyOwnerService.CreateCamera(args, user);

        return Ok("Camera created successfully.");
    }

    [HttpPost]
    [AuthorizationFilter(Constant.CreateDevice)]
    [Route("motionSensors")]
    public IActionResult CreateMotionSensor([FromBody] CreateDeviceRequest request)
    {
        const string deviceType = "MotionSensor";
        CreateDevice(request, deviceType);

        return Ok("Motion sensor created successfully.");
    }

    [HttpPost]
    [AuthorizationFilter(Constant.CreateDevice)]
    [Route("smartLamps")]
    public IActionResult CreateSmartLamp([FromBody] CreateDeviceRequest request)
    {
        const string deviceType = "SmartLamp";
        CreateDevice(request, deviceType);

        return Ok("Smart lamp created successfully.");
    }

    [HttpPost]
    [AuthorizationFilter(Constant.CreateDevice)]
    [Route("windowSensors")]
    public IActionResult CreateWindowSensor([FromBody] CreateDeviceRequest request)
    {
        const string deviceType = "WindowSensor";
        CreateDevice(request, deviceType);

        return Ok("Window sensor created successfully.");
    }

    [HttpPatch]
    [AuthorizationFilter(Constant.CompanyOwnerModifyRole)]
    [Route("changeCompanyOwnerRoles")]
    public IActionResult ChangeRoleToCompanyAndHomeOwner()
    {
        User user = GetLoggedUser();
        companyOwnerService.ChangeRoleToCompanyAndHomeOwner(user);

        return Ok("You change your role to CompanyAndHomeOwner.");
    }

    [HttpGet]
    [AuthorizationFilter(Constant.CreateCompany)]
    [Route("modelValidators")]
    public IActionResult GetModelValidators()
    {
        List<ShowModelValidatorsDto> validators = companyOwnerService.GetModelValidators();

        return Ok(validators);
    }

    [HttpPost]
    [AuthorizationFilter(Constant.CreateCompany)]
    [Route("{dllId}/importDevices")]
    public IActionResult ImportDevices([FromBody] ImportDevicesRequest request, [FromRoute] Guid dllId)
    {
        User user = GetLoggedUser();

        companyOwnerService.ImportDevices(dllId, request.Parameters, user);

        return Ok("Devices imported successfully.");
    }

    [HttpGet]
    [AuthorizationFilter(Constant.CreateCompany)]
    [Route("deviceImporters")]
    public IActionResult GetDeviceImporters()
    {
        List<ShowImporterDto> importers = companyOwnerService.GetDeviceImporters();

        return Ok(importers);
    }

    [HttpGet]
    [AuthorizationFilter(Constant.CreateCompany)]
    [Route("deviceImporters/{dllId}/parameters")]
    public IActionResult GetDeviceImporterParameters([FromRoute] Guid dllId)
    {
        List<ShowParamDto> parameters = companyOwnerService.GetDeviceImporterParameters(dllId);

        return Ok(parameters);
    }

    private void CreateDevice(CreateDeviceRequest request, string deviceType)
    {
        User user = GetLoggedUser();
        List<DeviceImage> images = GetImages(request.Images);
        var args = new CreateSmartDeviceWithoutCompanyArgs(request.Name, request.Model, request.Description,
            deviceType, images);

        companyOwnerService.CreateDevice(args, user);
    }

    private static List<DeviceImage> GetImages(List<DeviceImageRequest>? images)
    {
        return images == null ? [] : images.Select(image => new DeviceImage(image.ImageUrl, image.IsMain)).ToList();
    }
}
