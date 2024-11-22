namespace SmartHome.BusinessLogic.Models.ResponseDTOs;

public sealed class ShowImporterDto(Guid dllId, string dllName)
{
    public Guid DllId { get; set; } = dllId;
    public string DllName { get; set; } = dllName;
}
