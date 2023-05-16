using System.Text.Json.Serialization;

namespace LogParser.BLL.Models.IncomingDTO;

public class ScanInfo
{
    [JsonPropertyName("scanTime")]
    public DateTime ScanTime { get; set; }

    [JsonPropertyName("db")]
    public string Database { get; set; }

    [JsonPropertyName("server")]
    public string Server { get; set; }

    [JsonPropertyName("errorCount")]
    public int ErrorCount { get; set; }
}