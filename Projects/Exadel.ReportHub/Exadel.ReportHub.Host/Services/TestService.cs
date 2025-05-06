using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Handlers.TestEmail;
using Exadel.ReportHub.Host.Services.Abstract;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
[Route("api/test")]
public class TestService(ISender sender) : BaseService
{
    [HttpGet("send-email")]
    public async Task<ActionResult<TestEmailResult>> SendEmail([FromQuery] string email, [FromQuery] string subject, [FromQuery] string body)
    {
        var result = await sender.Send(new TestEmailRequest(email, subject, body));

        return FromResult(result);
    }
}
