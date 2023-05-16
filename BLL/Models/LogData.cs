using System.Text.Json.Serialization;

namespace LogParser.BLL.Models;

public class LogData
{
    [JsonPropertyName("scan")]
    public ScanInfo ScanInfo { get; set; }

    [JsonPropertyName("files")]
    public List<FileInfo> Files { get; set; }
}