using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Handlers.Item.Create;
using Exadel.ReportHub.Handlers.Item.Delete;
using Exadel.ReportHub.Handlers.Item.GetById;
using Exadel.ReportHub.Handlers.Item.Update;
using Exadel.ReportHub.SDK.DTOs.Item;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/items")]
public class ItemsService(ISender sender) : BaseService
{
    [Authorize(Policy = Constants.Authorization.Policy.Create)]
    [HttpPost]
    public async Task<IActionResult> AddItem([FromBody] CreateItemDTO createItemDto)
    {
        var result = await sender.Send(new CreateItemRequest(createItemDto));

        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Read)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetItemById([FromRoute] Guid id, [FromQuery][Required] Guid clientId)
    {
        var result = await sender.Send(new GetItemByIdRequest(id));

        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Update)]
    [HttpPut]
    public async Task<IActionResult> UpdateItemPrice([FromBody] UpdateItemDTO updateItemDTO)
    {
        var result = await sender.Send(new UpdateItemRequest(updateItemDTO));

        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Delete)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteItem([FromRoute] Guid id, [FromQuery][Required] Guid clientId)
    {
        var result = await sender.Send(new DeleteItemRequest(id));

        return FromResult(result);
    }
}
