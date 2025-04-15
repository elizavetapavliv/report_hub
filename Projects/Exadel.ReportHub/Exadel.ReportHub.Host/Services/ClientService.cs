using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

public class ClientService : BaseService
{
    [Authorize(Policy = Constants.Authorization.Policy.Read)]
    [HttpGet("{id:guid}")]
    public IActionResult ClientAdminReadTest([FromRoute] Guid id)
    {
        return Ok();
    }

    [Authorize(Policy = Constants.Authorization.Policy.Update)]
    [HttpPost("{id:guid}")]
    public IActionResult ClientAdminUpdateTest([FromRoute] Guid id, [FromBody] string someData)
    {
        return Ok();
    }
}
