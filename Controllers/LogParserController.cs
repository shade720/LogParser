using LogParser.BLL.Models.IncomingDTO;
using LogParser.BLL.Models.OutgoingDTO;
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
        return scanData.Files.Where(file => file.HasNoErrors == isCorrect);
    }

    // "api/errors" GET
    [HttpGet("errors")]
    public IEnumerable<FileErrorInfo> GetErrors([FromBody] ScanData scanData)
    {
        return scanData.Files
            .Where(file => !file.HasNoErrors)
            .Select(file => new FileErrorInfo
            {
                Filename = file.FileName,
                ErrorDescriptions = file.Errors.Select(err => err.ErrorText)
            });
    }

    // "api/errors/count" GET
    [HttpGet("errors/count")]
    public int GetErrorsCount([FromBody] ScanData scanData)
    {
        return scanData.ScanInfo.ErrorCount;
    }

    // "api/errors/{index}" GET
    [HttpGet("errors/{index}")]
    public IResult GetErrorById(int index, [FromBody] ScanData scanData)
    {
        try
        {
            var scannedFileInfoById = scanData.Files
                .Where(file => !file.HasNoErrors)
                .ToArray()[index];
            return Results.Ok(new FileErrorInfo
            {
                Filename = scannedFileInfoById.FileName,
                ErrorDescriptions = scannedFileInfoById.Errors.Select(err => err.ErrorText)
            });
        }
        catch (IndexOutOfRangeException e)
        {
            return Results.BadRequest(e.Message);
        }
    }
}