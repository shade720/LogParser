using System.Text.Json.Serialization;

namespace LogParser.BLL.Models;

public class ErrorInfo
{
    [JsonPropertyName("module")]
    public string Module { get; set; }

    [JsonPropertyName("ecode")]
    public int Code { get; set; }

    [JsonPropertyName("error")]
    public string ErrorText { get; set; }
}