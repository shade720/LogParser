using Microsoft.AspNetCore.Mvc;

namespace LogParser.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogParserController : ControllerBase
{
    // GET: api/<LogParserController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new[] { "value1", "value2" };
    }
}