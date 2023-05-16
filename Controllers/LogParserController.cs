using System.Text;
using System.Text.Json;
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

    // "api/query/check" GET
    [HttpGet("query/check")]
    public QueriesInfo GetQueryCheck([FromBody] ScanData scanData)
    {
        var queriesGroupedByScanResult = scanData.Files
            .Where(f => f.FileName.ToLower().StartsWith("query_"))
            .GroupBy(f => f.HasNoErrors)
            .ToDictionary(g => g.Key, g => g.ToList());

        if (!queriesGroupedByScanResult.ContainsKey(false))
            queriesGroupedByScanResult[false] = new List<ScannedFileInfo>();
        if (!queriesGroupedByScanResult.ContainsKey(true))
            queriesGroupedByScanResult[true] = new List<ScannedFileInfo>();

        return new QueriesInfo
        {
            TotalQueries = queriesGroupedByScanResult[true].Count + queriesGroupedByScanResult[false].Count,
            CorrectQueries = queriesGroupedByScanResult[true].Count,
            ErrorQueries = queriesGroupedByScanResult[false].Count,
            ErrorFiles = queriesGroupedByScanResult[false].Select(x => x.FileName)
        };
    }

    // "api/newErrors" POST
    [HttpPost("newErrors")]
    public async Task<IResult> PostNewErrors()
    {
        string scanDataJson;
        try
        {
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                scanDataJson = await reader.ReadToEndAsync();
            }
            JsonSerializer.Deserialize<ScanData>(scanDataJson);
        }
        catch
        {
            return Results.BadRequest("The JSON value could not be converted");
        }
        try
        {
            var saveFileName = Path.ChangeExtension(DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss"), ".json");
            await System.IO.File.WriteAllTextAsync(saveFileName, JsonSerializer.Serialize(scanDataJson));
        }
        catch
        {
            return Results.Problem("File was not saved", statusCode: 500);
        }
        return Results.Ok("Scan result successfully saved on disk!");
    }

    // "api/service/serviceInfo" GET
    [HttpGet("service/serviceInfo")]
    public ServiceInfo GetServiceInfo()
    {
        return new ServiceInfo();
    }
}