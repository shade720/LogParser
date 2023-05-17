using LogParser.BLL;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace LogParser.Controllers;

[Route("api")]
[ApiController]
public class LogSaverController : ControllerBase
{
    private readonly LogSaver _logSaver;

    public LogSaverController(LogSaver logSaver) => _logSaver = logSaver;

    // "api/newErrors" POST
    [HttpPost("newErrors")]
    public async Task<IResult> PostNewErrors()
    {
        try
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var scanDataJson = await reader.ReadToEndAsync();
            await _logSaver.SaveScanLogJson(scanDataJson);
        }
        catch (JsonException)
        {
            return Results.BadRequest("The JSON value could not be converted");
        }
        catch (IOException)
        {
            return Results.Problem("File was not saved", statusCode: 500);
        }
        return Results.Ok("Scan result successfully saved on disk!");
    }
}