using LogParser.BLL.Models.IncomingDTO;
using LogParser.BLL.Models.OutgoingDTO;

namespace LogParser.BLL;

public class LogFilter
{
    /// <summary>
    /// Возвращает журнал сканирования в неизменном виде.
    /// </summary>
    /// <param name="scanLog"> Журнал сканирования. </param>
    /// <returns> Журнал в неизменном виде. </returns>
    public ScanLog NoFilter(ScanLog scanLog)
    {
        return scanLog;
    }

    /// <summary>
    /// Выделяет информацию о сканировании из журнала. 
    /// </summary>
    /// <param name="scanLog"> Журнал сканирования. </param>
    /// <returns> Информация о сканировании. </returns>
    public ScanInfo FilterScanInfo(ScanLog scanLog)
    {
        return scanLog.ScanInfo;
    }

    /// <summary>
    /// Выделяет из журнала информацию о корректных/некорректных файлах.
    /// </summary>
    /// <param name="correctness"> Необходимо ли получить только корректные файлы или только некорректные. </param>
    /// <param name="scanLog"> Журнал сканирования. </param>
    /// <returns> Список файлов с информацией о сканировании. </returns>
    public IEnumerable<ScannedFileInfo> FilterFilesByСorrectness(bool correctness, ScanLog scanLog)
    {
        return scanLog.Files.Where(f => f.IsCorrect == correctness);
    }

    /// <summary>
    /// Выделяет из журнала информацию об ошибочных файлах.
    /// </summary>
    /// <param name="scanLog"> Журнал сканирования. </param>
    /// <returns> Список файлов с описанием присутствующих в них ошибок. </returns>
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

    /// <summary>
    /// Выделяет из журнала количество ошибок, найденых посредством сканирования. 
    /// </summary>
    /// <param name="scanLog"> Журнал сканирования. </param>
    /// <returns> Число ошибок, которое указано в журнале сканирования. </returns>
    public int FilterErrorsCount(ScanLog scanLog)
    {
        return scanLog.ScanInfo.ErrorCount;
    }

    /// <summary>
    /// Выделяет из журнала информацию о конкретном ошибочном файле с номером index.
    /// </summary>
    /// <param name="index"> Порядковый номер некорректного файла, который необходимо получить. </param>
    /// <param name="scanLog"> Журнал сканирования. </param>
    /// <returns> Информацию о файле с описанием присутствующих в нем ошибок. </returns>
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

    /// <summary>
    /// Выделяет из журнала сканирования информацию о файлах-запросах с указанием количества корректных файлов.
    /// </summary>
    /// <param name="scanLog"> Журнал сканирования. </param>
    /// <returns> Информацию о количестве корректных/некорректных файлах-запросах. </returns>
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