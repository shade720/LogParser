using LogParser.BLL.Models.OutgoingDTO;

namespace LogParser.BLL;

public class ServiceInfoProvider
{
    /// <summary>
    /// Предоставляет информацию о сервере.
    /// </summary>
    public ServiceInfo CurrentServiceInfo => new();
}