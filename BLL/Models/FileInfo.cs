using System.Text.Json.Serialization;

namespace LogParser.BLL.Models;

public class FileInfo
{
    [JsonPropertyName("filename")]
    public string FileName { get; set; }

    [JsonPropertyName("result")]
    public bool ScanResult { get; set; }

    [JsonPropertyName("errors")]
    public List<ErrorInfo> Errors { get; set; }

    [JsonPropertyName("scantime")]
    public DateTime ScanTime { get; set; }
}