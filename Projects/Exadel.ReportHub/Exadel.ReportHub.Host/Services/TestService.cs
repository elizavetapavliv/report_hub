﻿using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Handlers.Test;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
public class TestService(ISender sender) : BaseService
{
    [HttpGet]
    public IActionResult GetSampleAnswer()
    {
        return Ok();
    }

    [HttpGet("{getError}")]
    public async Task<IActionResult> GetTest([FromRoute] bool getError)
    {
        var result = await sender.Send(new GetRequest(getError));

        return FromResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddTest([FromBody] string name, [FromQuery] bool getError)
    {
        var result = await sender.Send(new CreateRequest(name, getError));

        return FromResult(result);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateTest([FromRoute] Guid id, [FromBody] string name, [FromQuery] bool getError)
    {
        var result = await sender.Send(new UpdateRequest(id, name, getError));

        return FromResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTest([FromRoute] Guid id, [FromQuery] bool getError)
    {
        var result = await sender.Send(new DeleteRequest(id, getError));

        return FromResult(result);
    }

    [Authorize(Policy = Constants.Policy.ClientAdmin)]
    [HttpGet("{clientId:guid}")]
    public IActionResult ClientAdminTest([FromRoute] Guid clientId)
    {
        return Ok();
    }
}
