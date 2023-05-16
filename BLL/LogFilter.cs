using LogParser.BLL.Models.IncomingDTO;
using LogParser.BLL.Models.OutgoingDTO;

namespace LogParser.BLL;

public class LogFilter
{
    public ScanLog NoFilter(ScanLog scanLog)
    {
        return scanLog;
    }

    public ScanInfo FilterScanInfo(ScanLog scanLog)
    {
        return scanLog.ScanInfo;
    }

    public IEnumerable<ScannedFileInfo> FilterScanInfoByСorrectness(bool correctness, ScanLog scanLog)
    {
        return scanLog.Files.Where(f => f.IsCorrect == correctness);
    }

    public IEnumerable<FileErrorInfo> FilterErrors(ScanLog scanLog)
    {
        return scanLog.Files
            .Where(file => !file.IsCorrect)
            .Select(file => new FileErrorInfo
            {
                Filename = file.FileName,
                ErrorDescriptions = file.Errors.Select(err => err.ErrorText)
            });
    }

    public int FilterErrorsCount(ScanLog scanLog)
    {
        return scanLog.ScanInfo.ErrorCount;
    }

    public FileErrorInfo FilterErrorsByFileIndex(int index, ScanLog scanLog)
    {
        var scannedFileInfoById = scanLog.Files
            .Where(file => !file.IsCorrect)
            .ToArray()[index];
        return new FileErrorInfo
        {
            Filename = scannedFileInfoById.FileName,
            ErrorDescriptions = scannedFileInfoById.Errors.Select(err => err.ErrorText)
        };
    }

    public QueriesInfo FilterQueries(ScanLog scanLog)
    {
        var queriesGroupedByScanResult = scanLog.Files
            .Where(f => f.FileName.ToLower().StartsWith("query_"))
            .GroupBy(f => f.IsCorrect)
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
}