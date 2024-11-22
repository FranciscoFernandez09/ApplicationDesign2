namespace SmartHome.BusinessLogic.Models.ResponseDTOs;

public sealed class ShowParamDto(string key, string value)
{
    public string Key { get; set; } = key;
    public string Value { get; set; } = value;
}
