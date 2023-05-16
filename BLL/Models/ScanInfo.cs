using System.Text.Json.Serialization;

namespace LogParser.BLL.Models;

public class ScanInfo
{
    [JsonPropertyName("scanTime")]
    public DateTime ScanTime { get; set; }

    [JsonPropertyName("db")]
    public string Db { get; set; }

    [JsonPropertyName("server")]
    public string Server { get; set; }

    [JsonPropertyName("errorCount")]
    public int ErrorCount { get; set; }
}