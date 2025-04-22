using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Handlers.Client.Create;
using Exadel.ReportHub.Handlers.Client.Delete;
using Exadel.ReportHub.Handlers.Client.Get;
using Exadel.ReportHub.Handlers.Client.GetById;
using Exadel.ReportHub.Handlers.Client.UpdateName;
using Exadel.ReportHub.Host.Infrastructure.Models;
using Exadel.ReportHub.Host.Services.Abstract;
using Exadel.ReportHub.SDK.DTOs.Client;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
[Route("api/clients")]
public class ClientsService(ISender sender) : BaseService
{
    /// <summary>
    /// Creates a new client.
    /// </summary>
    /// <param name="createClientDto"></param>
    /// <returns>The created client object.</returns>
    /// <response code="200">Client created successfully.</response>
    /// <response code="400">Invalid input provided.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. Users who do not have permission to perform this action.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Create)]
    [HttpPost]
    [ProducesResponseType(typeof(ClientDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ClientDTO>> AddClient([FromBody] CreateClientDTO createClientDto)
    {
        var result = await sender.Send(new CreateClientRequest(createClientDto));

        return FromResult(result);
    }

    /// <summary>
    /// Gets a client by their unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The client matching the specified ID.</returns>
    /// <response code="200">Client found and returned successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. Users who do not have permission to perform this action.</response>
    /// <response code="404">Client with the given ID was not found.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Read)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ClientDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientDTO>> GetClientById([FromRoute] Guid id)
    {
        var result = await sender.Send(new GetClientByIdRequest(id));

        return FromResult(result);
    }

    /// <summary>
    /// Gets a list of all clients.
    /// </summary>
    /// <returns>A list of all registered clients.</returns>
    /// <response code="200">List of clients returned successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. Users who not have permission to perform this action.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Read)]
    [HttpGet]
    [ProducesResponseType(typeof(IList<ClientDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IList<ClientDTO>>> GetClients()
    {
        var result = await sender.Send(new GetClientsRequest());

        return FromResult(result);
    }

    /// <summary>
    /// Updates the name of a specific client.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <returns>No content.</returns>
    /// <response code="204">Client name updated successfully.</response>
    /// <response code="400">Invalid name provided.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. Users who do not have permission to perform this action.</response>
    /// <response code="404">Client with the given ID was not found.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Update)]
    [HttpPatch("{id:guid}/name")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateClientName([FromRoute] Guid id, [FromBody] string name)
    {
        var result = await sender.Send(new UpdateClientNameRequest(id, name));

        return FromResult(result);
    }

    /// <summary>
    /// Deletes a client by their unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>No content.</returns>
    /// <response code="204">Client deleted successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. Users who do not have permission to perform this action.</response>
    /// <response code="404">Client with the given ID was not found.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Delete)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteClient([FromRoute] Guid id)
    {
        var result = await sender.Send(new DeleteClientRequest(id));

        return FromResult(result);
    }
}
