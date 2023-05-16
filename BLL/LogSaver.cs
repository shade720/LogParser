using LogParser.BLL.Models.IncomingDTO;
using System.Text.Json;

namespace LogParser.BLL;

public class LogSaver
{
    private const string SaveFileNameFormat = "dd-MM-yyyy_hh-mm-ss";

    public async Task SaveScanLogJson(string scanLogJson)
    {
        try
        {
            JsonSerializer.Deserialize<ScanLog>(scanLogJson);
            var saveFileName = Path.ChangeExtension(DateTime.Now.ToString(SaveFileNameFormat), ".json");
            await File.WriteAllTextAsync(saveFileName, scanLogJson);
        }
        catch (JsonException)
        {
            throw new JsonException("The JSON value could not be converted");
        }
        catch (Exception)
        {
            throw new FileLoadException("File was not saved");
        }
    }
}