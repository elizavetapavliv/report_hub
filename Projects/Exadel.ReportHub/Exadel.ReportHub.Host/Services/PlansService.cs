using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Handlers.Plan.Create;
using Exadel.ReportHub.Handlers.Plan.Delete;
using Exadel.ReportHub.Handlers.Plan.Get;
using Exadel.ReportHub.Handlers.Plan.GetById;
using Exadel.ReportHub.Handlers.Plan.UpdateAmount;
using Exadel.ReportHub.Handlers.Plan.UpdateDate;
using Exadel.ReportHub.SDK.DTOs.Plan;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
[Route("api/plans")]
[ApiController]
public class PlansService(ISender sender) : BaseService
{
    [Authorize(Policy = Constants.Authorization.Policy.Read)]
    [HttpGet]
    public async Task<IActionResult> GetPlans()
    {
        var result = await sender.Send(new GetPlansRequest());
        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Read)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPlanById([FromRoute] Guid id)
    {
        var result = await sender.Send(new GetPlansByIdRequest(id));
        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Create)]
    [HttpPost]
    public async Task<IActionResult> AddPlan([FromBody] CreatePlanDTO createPlanDto)
    {
        var result = await sender.Send(new CreatePlanRequest(createPlanDto));
        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Delete)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePlan([FromRoute] Guid id)
    {
        var result = await sender.Send(new DeletePlanRequest(id));
        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Update)]
    [HttpPatch("{id:guid}/amount")]
    public async Task<IActionResult> UpdateAmount([FromRoute] Guid id, [FromBody] int amount)
    {
        var result = await sender.Send(new UpdatePlanAmountRequest(id, amount));
        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Update)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePlan([FromRoute] Guid id, [FromBody] UpdatePlanDateDTO updatePlanDateDto)
    {
        var result = await sender.Send(new UpdatePlanDateRequest(id, updatePlanDateDto));
        return FromResult(result);
    }
}
