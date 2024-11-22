namespace Importer.DTOs;

public sealed class ImageImporterDto(string url, bool isMain)
{
    public string Url { get; set; } = url;
    public bool IsMain { get; set; } = isMain;
}
