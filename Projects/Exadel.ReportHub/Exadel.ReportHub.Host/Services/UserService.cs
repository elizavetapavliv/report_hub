﻿using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Handlers.User.Create;
using Exadel.ReportHub.Handlers.User.DeleteUser;
using Exadel.ReportHub.Handlers.User.Get;
using Exadel.ReportHub.Handlers.User.GetById;
using Exadel.ReportHub.Handlers.User.UpdateActivity;
using Exadel.ReportHub.Handlers.User.UpdateName;
using Exadel.ReportHub.Handlers.User.UpdatePassword;
using Exadel.ReportHub.SDK.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/users")]
public class UserService(ISender sender) : BaseService
{
    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] CreateUserDTO createUserDto)
    {
        var result = await sender.Send(new CreateUserRequest(createUserDto));

        return FromResult(result, StatusCodes.Status201Created);
    }

    [Authorize(Policy = Constants.Authorization.Policy.AllUsers)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var result = await sender.Send(new GetUserByIdRequest(id));

        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.AllUsers)]
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] bool? isActive)
    {
        var result = await sender.Send(new GetUsersRequest(isActive));

        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.SuperAdmin)]
    [HttpPatch("{id:guid}/activity")]
    public async Task<IActionResult> UpdateUserActivity([FromRoute] Guid id, [FromBody] bool isActive)
    {
        var result = await sender.Send(new UpdateUserActivityRequest(id, isActive));

        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.AllUsers)]
    [HttpPatch("password")]
    public async Task<IActionResult> UpdateUserPassword([FromBody] string password)
    {
        var result = await sender.Send(new UpdateUserPasswordRequest(password));
        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.SuperAdmin)]
    [HttpPatch("{id:guid}/fullname")]
    public async Task<IActionResult> UpdateUserFullName([FromRoute] Guid id, [FromBody] string fullName)
    {
        var result = await sender.Send(new UpdateUserNameRequest(id, fullName));
        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.SuperAdmin)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var result = await sender.Send(new DeleteUserRequest(id));
        return FromResult(result);
    }
}
