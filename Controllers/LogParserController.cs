using System.Text;
using System.Text.Json;
using LogParser.BLL;
using LogParser.BLL.Models.IncomingDTO;
using LogParser.BLL.Models.OutgoingDTO;
using Microsoft.AspNetCore.Mvc;

namespace LogParser.Controllers;

[Route("[controller]/api")]
[ApiController]
public class LogParserController : ControllerBase
{
    private readonly LogFilter _logFilter;
    private readonly LogSaver _logSaver;
    private readonly ServiceInfoProvider _serviceInfoProvider;

    public LogParserController(LogFilter logFilter, LogSaver logSaver, ServiceInfoProvider serviceInfoProvider)
    {
        _logFilter = logFilter;
        _logSaver = logSaver;
        _serviceInfoProvider = serviceInfoProvider;
    }

    // "api/allData" GET
    [HttpGet ("allData")]
    public ScanLog GetAllData([FromBody] ScanLog scanLog)
    {
        return _logFilter.NoFilter(scanLog);
    }

    // "api/scan" GET
    [HttpGet("scan")]
    public ScanInfo GetScanInfo([FromBody] ScanLog scanLog)
    {
        return _logFilter.FilterScanInfo(scanLog);
    }

    // "api/filenames?correct={value}" GET
    [HttpGet("filenames")]
    public IEnumerable<ScannedFileInfo> GetScanInfo([FromQuery (Name = "correct")] bool isCorrect, [FromBody] ScanLog scanLog)
    {
        return _logFilter.FilterScanInfoByСorrectness(isCorrect, scanLog);
    }

    // "api/errors" GET
    [HttpGet("errors")]
    public IEnumerable<FileErrorInfo> GetErrors([FromBody] ScanLog scanLog)
    {
        return _logFilter.FilterErrors(scanLog);
    }

    // "api/errors/count" GET
    [HttpGet("errors/count")]
    public int GetErrorsCount([FromBody] ScanLog scanLog)
    {
        return _logFilter.FilterErrorsCount(scanLog);
    }

    // "api/errors/{index}" GET
    [HttpGet("errors/{index}")]
    public IResult GetErrorById(int index, [FromBody] ScanLog scanLog)
    {
        try
        {
            var fileErrorInfo = _logFilter.FilterErrorsByFileIndex(index, scanLog);
            return Results.Ok(fileErrorInfo);
        }
        catch (IndexOutOfRangeException)
        {
            return Results.BadRequest("File with such an index does not exist");
        }
    }

    // "api/query/check" GET
    [HttpGet("query/check")]
    public QueriesInfo GetQueryCheck([FromBody] ScanLog scanLog)
    {
        return _logFilter.FilterQueries(scanLog);
    }

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
        catch (JsonException exception)
        {
            return Results.BadRequest(exception.Message);
        }
        catch (Exception exception)
        {
            return Results.Problem(exception.Message, statusCode: 500);
        }
        return Results.Ok("Scan result successfully saved on disk!");
    }

    // "api/service/serviceInfo" GET
    [HttpGet("service/serviceInfo")]
    public ServiceInfo GetServiceInfo()
    {
        return _serviceInfoProvider.CurrentServiceInfo;
    }
}