using LogParser.BLL;
using LogParser.BLL.Models.IncomingDTO;
using LogParser.BLL.Models.OutgoingDTO;
using Microsoft.AspNetCore.Mvc;

namespace LogParser.Controllers;

[Route("api")]
[ApiController]
public class LogParserController : ControllerBase
{
    private readonly LogFilter _logFilter;
    public LogParserController(LogFilter logFilter) => _logFilter = logFilter;

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
}