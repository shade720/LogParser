namespace LogParser.BLL.Models.OutgoingDTO;

public class ServiceInfo
{
    public string? AppName { get; set; } = typeof(Program).Assembly.GetName().Name;
    public string? Version { get; set; } = typeof(Program).Assembly.GetName().Version?.ToString();
    public DateTime DateUtc { get; set; } = DateTime.UtcNow;
}