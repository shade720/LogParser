using LogParser.BLL;
using LogParser.BLL.Models.OutgoingDTO;
using Microsoft.AspNetCore.Mvc;

namespace LogParser.Controllers;

[Route("api/service")]
[ApiController]
public class ServiceController : ControllerBase
{
    private readonly ServiceInfoProvider _serviceInfoProvider;

    public ServiceController(ServiceInfoProvider serviceInfoProvider) => _serviceInfoProvider = serviceInfoProvider;

    // "api/service/serviceInfo" GET
    [HttpGet("serviceInfo")]
    public ServiceInfo GetServiceInfo()
    {
        return _serviceInfoProvider.CurrentServiceInfo;
    }
}