using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Handlers.Item.Create;
using Exadel.ReportHub.Handlers.Item.Delete;
using Exadel.ReportHub.Handlers.Item.GetByClientId;
using Exadel.ReportHub.Handlers.Item.GetById;
using Exadel.ReportHub.Handlers.Item.Update;
using Exadel.ReportHub.Host.Infrastructure.Models;
using Exadel.ReportHub.Host.Services.Abstract;
using Exadel.ReportHub.SDK.DTOs.Item;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
[Route("api/items")]
public class ItemsService(ISender sender) : BaseService
{
    /// <summary>
    /// Creates a new item.
    /// </summary>
    /// <param name="createItemDto"></param>
    /// <returns>The newly created item details.</returns>
    /// <response code="200">Item created successfully.</response>
    /// <response code="400">Invalid input provided.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. User does not have permission to perform this action.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Create)]
    [HttpPost]
    [ProducesResponseType(typeof(ItemDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ItemDTO>> AddItem([FromBody] CreateUpdateItemDTO createItemDto)
    {
        var result = await sender.Send(new CreateItemRequest(createItemDto));

        return FromResult(result);
    }

    /// <summary>
    /// Retrieves a list of items for a specific client.
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns>A list of the client's items.</returns>
    /// <response code="200">Items retrieved successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. User does not have permission to view these items.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Read)]
    [HttpGet]
    [ProducesResponseType(typeof(IList<ItemDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IList<ItemDTO>>> GetItemsByClientId([FromQuery][Required] Guid clientId)
    {
        var result = await sender.Send(new GetItemsByClientIdRequest(clientId));

        return FromResult(result);
    }

    /// <summary>
    /// Retrieves a specific item by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="clientId"></param>
    /// <returns>The item details.</returns>
    /// <response code="200">Item retrieved successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. User does not have permission to view this item.</response>
    /// <response code="404">Item not found.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Read)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ItemDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ItemDTO>> GetItemById([FromRoute] Guid id, [FromQuery][Required] Guid clientId)
    {
        var result = await sender.Send(new GetItemByIdRequest(id));

        return FromResult(result);
    }

    /// <summary>
    /// Updates the details of an existing item.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateItemDTO"></param>
    /// <returns>No content</returns>
    /// <response code="204">Item updated successfully.</response>
    /// <response code="400">Invalid input provided.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. User does not have permission to update this item.</response>
    /// <response code="404">Item not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = Constants.Authorization.Policy.Update)]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateItem([FromRoute] Guid id, [FromBody] CreateUpdateItemDTO updateItemDTO)
    {
        var result = await sender.Send(new UpdateItemRequest(id, updateItemDTO));

        return FromResult(result);
    }

    /// <summary>
    /// Deletes an existing item by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="clientId"></param>
    /// <returns>No content</returns>
    /// <response code="204">Item deleted successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. User does not have permission to delete this item.</response>
    /// <response code="404">Item not found.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Delete)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteItem([FromRoute] Guid id, [FromQuery][Required] Guid clientId)
    {
        var result = await sender.Send(new DeleteItemRequest(id));

        return FromResult(result);
    }
}
