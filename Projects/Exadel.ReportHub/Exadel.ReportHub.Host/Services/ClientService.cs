﻿using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Handlers.Client.Create;
using Exadel.ReportHub.Handlers.Client.Delete;
using Exadel.ReportHub.Handlers.Client.Get;
using Exadel.ReportHub.Handlers.Client.UpdateActivity;
using Exadel.ReportHub.Handlers.Client.UpdateName;
using Exadel.ReportHub.SDK.DTOs.Client;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/client")]
public class ClientService(ISender sender) : BaseService
{
    [HttpPost]
    public async Task<IActionResult> AddClient([FromBody] CreateClientDTO createClientDto)
    {
        var result = await sender.Send(new CreateClientRequest(createClientDto));

        return FromResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetClientById([FromRoute] Guid id)
    {
        var result = await sender.Send(new GetClientByIdRequest(id));

        return FromResult(result);
    }

    [HttpPatch("{id:guid}/activity")]
    public async Task<IActionResult> UpdateClientActivity([FromRoute] Guid id, [FromBody] bool isActive)
    {
        var result = await sender.Send(new UpdateClientActivityRequest(id, isActive));

        return FromResult(result);
    }

    [HttpPatch("{id:guid}/name")]
    public async Task<IActionResult> UpdateClientName([FromRoute] Guid id, [FromBody] string name)
    {
        var result = await sender.Send(new UpdateClientNameRequest(id, name));

        return FromResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteClient([FromRoute] Guid id)
    {
        var result = await sender.Send(new DeleteClientRequest(id));

        return FromResult(result);
    }
}
