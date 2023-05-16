using System.Text.Json.Serialization;

namespace LogParser.BLL.Models.IncomingDTO;

public class ScannedFileInfo
{
    [JsonPropertyName("filename")]
    public string FileName { get; set; }

    [JsonPropertyName("result")]
    public bool HasNoErrors { get; set; }

    [JsonPropertyName("errors")]
    public List<ErrorInfo> Errors { get; set; }

    [JsonPropertyName("scantime")]
    public DateTime ScanTime { get; set; }
}