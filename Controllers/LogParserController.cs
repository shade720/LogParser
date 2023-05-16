using LogParser.BLL.Models.IncomingDTO;
using Microsoft.AspNetCore.Mvc;

namespace LogParser.Controllers;

[Route("[controller]/api")]
[ApiController]
public class LogParserController : ControllerBase
{
    // "api/allData" GET
    [HttpGet ("allData")]
    public ScanData GetAllData([FromBody] ScanData scanData)
    {
        return scanData;
    }

    // "api/scan" GET
    [HttpGet("scan")]
    public ScanInfo GetScanInfo([FromBody] ScanData scanData)
    {
        return scanData.ScanInfo;
    }

    // "api/filenames?correct={value}" GET
    [HttpGet("filenames")]
    public IEnumerable<ScannedFileInfo> GetScanInfo(
        [FromQuery (Name = "correct")] bool isCorrect, 
        [FromBody] ScanData scanData)
    {
        return scanData.Files.Where(file => file.IsContainErrors == isCorrect);
    }
}