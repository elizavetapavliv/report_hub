﻿using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using MediatR;

namespace Exadel.ReportHub.Handlers.UserHandlers.Get;

public record GetUserByIdRequest(Guid Id) : IRequest<ErrorOr<UserDTO>>;

public class GetUserByIdHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdRequest, ErrorOr<UserDTO>>
{
    public async Task<ErrorOr<UserDTO>> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null)
        {
            return await Task.FromResult<ErrorOr<UserDTO>>(Error.NotFound());
        }

        var userDTO = new UserDTO
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName
        };

        return await Task.FromResult<ErrorOr<UserDTO>>(userDTO);
    }
}
