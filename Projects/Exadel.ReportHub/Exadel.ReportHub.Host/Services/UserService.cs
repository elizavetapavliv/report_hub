using Exadel.ReportHub.Handlers.Test;
using Exadel.ReportHub.Handlers.UserHandlers;
using Exadel.ReportHub.SDK.DTOs.UserDTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

public class UserService(ISender sender) : BaseService
{
    [HttpPost("createUser")]
    public async Task<IActionResult> AddUser([FromBody] CreateUserDTO createUserDTO)
    {
        var createUserRequest = new CreateUserRequest(createUserDTO.Email, createUserDTO.FullName, createUserDTO.Password);
        var result = await sender.Send(createUserRequest);

        return FromResult(result);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var getUserByIdRequest = new GetUserByIdRequest(id);
        var result = await sender.Send(getUserByIdRequest);

        return FromResult(result);
    }

    [HttpGet("getAllUser")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await sender.Send(new GetAllUsersRequest());
        return FromResult(result);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateUserActivity([FromRoute] Guid id, [FromBody] bool isActive)
    {
        var updateUserActivityRequest = new UpdateUserActivityRequest(id, isActive);
        var result = await sender.Send(updateUserActivityRequest);

        return FromResult(result);
    }
}
