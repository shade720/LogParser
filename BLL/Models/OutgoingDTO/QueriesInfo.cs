using System.Text.Json.Serialization;

namespace LogParser.BLL.Models.OutgoingDTO;

public class QueriesInfo
{
    [JsonPropertyName("total")]
    public int TotalQueries { get; set; }

    [JsonPropertyName("correct")]
    public int CorrectQueries { get; set; }

    [JsonPropertyName("errors")]
    public int ErrorQueries { get; set; }

    [JsonPropertyName("filenames")]
    public IEnumerable<string> ErrorFiles { get; set; }
}