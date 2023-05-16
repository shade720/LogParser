using System.Text.Json.Serialization;

namespace LogParser.BLL.Models.IncomingDTO;

public class ScanData
{
    [JsonPropertyName("scan")]
    public ScanInfo ScanInfo { get; set; }

    [JsonPropertyName("files")]
    public List<ScannedFileInfo> Files { get; set; }
}