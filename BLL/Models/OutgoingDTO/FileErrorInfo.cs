namespace LogParser.BLL.Models.OutgoingDTO;

public class FileErrorInfo
{
    public string Filename { get; set; }
    public IEnumerable<string> ErrorDescriptions { get; set; }
}