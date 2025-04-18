using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
public class PingService : BaseService
{
    [HttpGet]
    public IActionResult Ping()
    {
        return Ok();
    }
}
