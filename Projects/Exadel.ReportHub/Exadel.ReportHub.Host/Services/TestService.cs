using Exadel.ReportHub.Handlers.Features.TestFeatures.Commands.AddTest;
using Exadel.ReportHub.Handlers.Features.TestFeatures.Commands.DeleteTest;
using Exadel.ReportHub.Handlers.Features.TestFeatures.Commands.UpdateTest;
using Exadel.ReportHub.Handlers.Features.TestFeatures.Queries.GetTest;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

public class TestService : BaseService
{
    [HttpGet]
    public IActionResult GetSampleAnswer()
    {
        return Ok();
    }

    [HttpGet("{getError}")]
    public async Task<IActionResult> GetTest([FromRoute] GetRequest request)
    {
        var result = await Mediator.Send(request);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors));
    }

    [HttpPost]
    public async Task<IActionResult> AddTest([FromBody] AddRequest request)
    {
        var result = await Mediator.Send(request);

        return result.Match(
            success => Created(),
            errors => Problem(errors));
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateTest([FromRoute] Guid id, [FromBody] AddRequest request)
    {
        var updateRequest = new UpdateRequest(id, request.name, request.getError);
        var result = await Mediator.Send(updateRequest);

        return result.Match(
            success => NoContent(),
            errors => Problem(errors));
    }

    [HttpDelete("{id:guid}/{getError}")]
    public async Task<IActionResult> DeleteTest([FromRoute] DeleteRequest request)
    {
        var result = await Mediator.Send(request);

        return result.Match(
            success => NoContent(),
            errors => Problem(errors));
    }
}
