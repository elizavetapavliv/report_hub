using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Handlers.Plan.Create;
using Exadel.ReportHub.Handlers.Plan.Delete;
using Exadel.ReportHub.Handlers.Plan.GetByClientId;
using Exadel.ReportHub.Handlers.Plan.GetById;
using Exadel.ReportHub.Handlers.Plan.Update;
using Exadel.ReportHub.Host.Infrastructure.Models;
using Exadel.ReportHub.Host.Services.Abstract;
using Exadel.ReportHub.SDK.DTOs.Plan;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
[Route("api/plans")]
public class PlansService(ISender sender) : BaseService
{
    /// <summary>
    /// Creates a new plan.
    /// </summary>
    /// <param name="createPlanDto"></param>
    /// <returns>The newly created plan details.</returns>
    /// <response code="200">Plan created successfully.</response>
    /// <response code="400">Invalid input provided.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. User does not have permission to perform this action.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Create)]
    [HttpPost]
    [ProducesResponseType(typeof(PlanDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PlanDTO>> AddPlan([FromBody] CreatePlanDTO createPlanDto)
    {
        var result = await sender.Send(new CreatePlanRequest(createPlanDto));
        return FromResult(result);
    }

    /// <summary>
    /// Retrieves a list of plans for a specific client.
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns>A list of the client's plans.</returns>
    /// <response code="200">Plans retrieved successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. User does not have permission to view these plans.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Read)]
    [HttpGet]
    [ProducesResponseType(typeof(IList<PlanDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IList<PlanDTO>>> GetPlansByClientId([FromQuery][Required] Guid clientId)
    {
        var result = await sender.Send(new GetPlansByClientIdRequest(clientId));
        return FromResult(result);
    }

    /// <summary>
    /// Retrieves a specific plan by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="clientId"></param>
    /// <returns>The plan details.</returns>
    /// <response code="200">Plan retrieved successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. User does not have permission to view this plan.</response>
    /// <response code="404">Plan not found.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Read)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PlanDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlanDTO>> GetPlanById([FromRoute] Guid id, [FromQuery][Required] Guid clientId)
    {
        var result = await sender.Send(new GetPlanByIdRequest(id));
        return FromResult(result);
    }

    /// <summary>
    /// Updates an existing plan.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updatePlanDateDto"></param>
    /// <returns>No content</returns>
    /// <response code="204">Plan updated successfully.</response>
    /// <response code="400">Invalid input provided.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. User does not have permission to update this plan.</response>
    /// <response code="404">Plan not found.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Update)]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdatePlan([FromRoute] Guid id, [FromBody] UpdatePlanDTO updatePlanDateDto)
    {
        var result = await sender.Send(new UpdatePlanRequest(id, updatePlanDateDto));
        return FromResult(result);
    }

    /// <summary>
    /// Deletes an existing plan by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="clientId"></param>
    /// <returns>No content</returns>
    /// <response code="204">Plan deleted successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. User does not have permission to delete this plan.</response>
    /// <response code="404">Plan not found.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Delete)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeletePlan([FromRoute] Guid id, [FromQuery][Required] Guid clientId)
    {
        var result = await sender.Send(new DeletePlanRequest(id));
        return FromResult(result);
    }
}
