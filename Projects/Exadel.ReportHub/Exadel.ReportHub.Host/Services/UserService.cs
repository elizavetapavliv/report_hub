﻿using Exadel.ReportHub.Handlers.User.Create;
using Exadel.ReportHub.Handlers.User.Get;
using Exadel.ReportHub.Handlers.User.GetAllActive;
using Exadel.ReportHub.Handlers.User.UpdateActivity;
using Exadel.ReportHub.SDK.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ApiController]
[Route("api/users")]
public class UserService(ISender sender) : BaseService
{
    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] CreateUserDto createUserDto)
    {
        var result = await sender.Send(new CreateUserRequest(createUserDto));

        return FromResult(result, StatusCodes.Status201Created);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var result = await sender.Send(new GetUserByIdRequest(id));

        return FromResult(result);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveUsers()
    {
        var result = await sender.Send(new GetActiveUsersRequest());

        return FromResult(result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateUserActivity([FromRoute] Guid id, [FromBody] bool isActive)
    {
        var result = await sender.Send(new UpdateUserActivityRequest(id, isActive));

        return FromResult(result);
    }
}
