using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Handlers.UserAssignment.Upsert;
using Exadel.ReportHub.Host.Infrastructure.Models;
using Exadel.ReportHub.Host.Services.Abstract;
using Exadel.ReportHub.SDK.DTOs.UserAssignment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
[Route("api/user-assignments")]
public class UserAssignmentsService(ISender sender) : BaseService
{
    /// <summary>
    /// Creates a new user assignment or updates an existing one if it already exists.
    /// </summary>
    /// <param name="upsertUserAssignmentDto"></param>
    /// <returns>No content</returns>
    /// <response code="204">User assignment was successfully created or updated.</response>
    /// <response code="400">The request contains invalid data.</response>
    /// <response code="401">Unauthorized access. Authentication is required.</response>
    /// <response code="403">Forbidden. The user does not have permission to perform this action.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [HttpPost]
    public async Task<ActionResult> UpsertUserAssignment([FromBody] UpsertUserAssignmentDTO upsertUserAssignmentDto)
    {
        var result = await sender.Send(new UpsertUserAssignmentRequest(upsertUserAssignmentDto));
        return FromResult(result);
    }
}
