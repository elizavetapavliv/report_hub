using Exadel.ReportHub.Handlers.UserHandlers.Create;
using Exadel.ReportHub.Handlers.UserHandlers.Get;
using Exadel.ReportHub.Handlers.UserHandlers.GetAllActive;
using Exadel.ReportHub.Handlers.UserHandlers.UpdateActivity;
using Exadel.ReportHub.SDK.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ApiController]
[Route("api/users")]
public class UserService(ISender sender) : BaseService
{
    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] CreateUserDTO createUserDto)
    {
        var createUserRequest = new CreateUserRequest(createUserDto.Email, createUserDto.FullName, createUserDto.Password);
        var result = await sender.Send(createUserRequest);

        return FromResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var getByIdRequest = new GetUserByIdRequest(id);
        var result = await sender.Send(getByIdRequest);

        return FromResult(result);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveUsers()
    {
        var result = await sender.Send(new GetActiveUsersRequest());
        return FromResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUserActivity([FromRoute] Guid id, [FromBody] bool isActive)
    {
        var updateUserActivityRequest = new UpdateUserActivityRequest(id, isActive);
        var result = await sender.Send(updateUserActivityRequest);

        return FromResult(result);
    }
}
