using LogParser.BLL.Models.IncomingDTO;
using System.Text.Json;

namespace LogParser.BLL;

public class LogSaver
{
    private const string SaveFileNameFormat = "dd-MM-yyyy_hh-mm-ss";

    /// <summary>
    /// Сохраняет журнал сканирования в файл в формате JSON, если строка представляет собой объект ScanLog.
    /// </summary>
    /// <param name="scanLogJson"> JSON строка, которую необходимо сохранить на диске. </param>
    /// <returns></returns>
    /// <exception cref="JsonException"> Исключение выбрасывается в случае неудачного конвертирования строки в объект ScanLog.
    /// Содержание: "The JSON value could not be converted" </exception>
    /// <exception cref="FileLoadException"> Исключение выбрасывается в случае неудачной записи файла.
    /// Содержание: "File was not saved" </exception>
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