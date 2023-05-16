using LogParser.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace LogParser.Controllers;

[Route("[controller]/api")]
[ApiController]
public class LogParserController : ControllerBase
{
    // "api/allData" GET
    [HttpGet ("allData")]
    public LogData GetAllData([FromBody] LogData logData)
    {
        return logData;
    }

    // "api/scan" GET
    [HttpGet("scan")]
    public ScanInfo GetScanInfo([FromBody] LogData logData)
    {
        return logData.ScanInfo;
    }
}