﻿using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Handlers.User.Create;
using Exadel.ReportHub.Handlers.User.Delete;
using Exadel.ReportHub.Handlers.User.Get;
using Exadel.ReportHub.Handlers.User.GetById;
using Exadel.ReportHub.Handlers.User.UpdateActivity;
using Exadel.ReportHub.Handlers.User.UpdateName;
using Exadel.ReportHub.Handlers.User.UpdatePassword;
using Exadel.ReportHub.Host.Infrastructure.Models;
using Exadel.ReportHub.Host.Services.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
[Route("api/users")]
public class UsersService(ISender sender) : BaseService
{
    [Authorize(Policy = Constants.Authorization.Policy.Create)]
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new user", Description = "Creates a new user with the provided details.")]
    [SwaggerResponse(StatusCodes.Status201Created, "User created successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid user data", typeof(ErrorResponse))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication is required to access this endpoint")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "this User does not have permission to create a user")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserDTO>> AddUser([FromBody] CreateUserDTO createUserDto)
    {
        var result = await sender.Send(new CreateUserRequest(createUserDto));

        return FromResult(result, StatusCodes.Status201Created);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Read)]
    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get user details", Description = "Retrieves the details of a user by their unique ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "User details retrieved successfully")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication is required to access this endpoint")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "this User does not have permission to access the user")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found", typeof(ErrorResponse))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserDTO>> GetUserById([FromRoute] Guid id)
    {
        var result = await sender.Send(new GetUserByIdRequest(id));

        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Read)]
    [HttpGet]
    [SwaggerOperation(Summary = "Get list of users", Description = "Retrieves a list of users, optionally filtered by their active status.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Users retrieved successfully")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication is required to access this endpoint")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "this User does not have permission to access the users")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<UserDTO>>> GetUsers([FromQuery] bool? isActive)
    {
        var result = await sender.Send(new GetUsersRequest(isActive));

        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Update)]
    [HttpPatch("{id:guid}/activity")]
    [SwaggerOperation(Summary = "Update user activity status", Description = "Updates the activity status of a user.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User activity status changed successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data provided", typeof(ErrorResponse))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication is required to access this endpoint")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "this User does not have permission to update the activity status")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found", typeof(ErrorResponse))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateUserActivity([FromRoute] Guid id, [FromBody] bool isActive)
    {
        var result = await sender.Send(new UpdateUserActivityRequest(id, isActive));

        return FromResult(result);
    }

    [Authorize]
    [HttpPatch("password")]
    [SwaggerOperation(Summary = "Update user password", Description = "Updates the password of the currently authenticated user.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User password changed successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid password data", typeof(ErrorResponse))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication is required to access this endpoint")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateUserPassword([FromBody] string password)
    {
        var result = await sender.Send(new UpdateUserPasswordRequest(password));
        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Update)]
    [HttpPatch("{id:guid}/fullname")]
    [SwaggerOperation(Summary = "Update user full name", Description = "Updates the full name of the user specified by ID.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User full name changed successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid name data", typeof(ErrorResponse))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication is required to access this endpoint")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "this User does not have permission to update the full name")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found", typeof(ErrorResponse))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateUserFullName([FromRoute] Guid id, [FromBody] string fullName)
    {
        var result = await sender.Send(new UpdateUserNameRequest(id, fullName));
        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Delete)]
    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete user", Description = "Deletes a user by their unique ID.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User deleted successfully")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication is required to access this endpoint")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "this User does not have permission to delete the user")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found", typeof(ErrorResponse))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteUser([FromRoute] Guid id)
    {
        var result = await sender.Send(new DeleteUserRequest(id));
        return FromResult(result);
    }
}
