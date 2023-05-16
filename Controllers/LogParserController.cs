using LogParser.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace LogParser.Controllers;

[Route("[controller]/api")]
[ApiController]
public class LogParserController : ControllerBase
{
    // "api/allData" GET
    [HttpGet ("allData")]
    public LogData Get([FromBody] LogData logData)
    {
        return logData;
    }
}