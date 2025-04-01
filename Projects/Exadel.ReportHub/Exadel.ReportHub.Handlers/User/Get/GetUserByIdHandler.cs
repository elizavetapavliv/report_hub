﻿using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using MediatR;

namespace Exadel.ReportHub.Handlers.User.Get;

public record GetUserByIdRequest(Guid Id) : IRequest<ErrorOr<UserDTO>>;

public class GetUserByIdHandler(IUserRepository userRepository, IMapper _mapper) : IRequestHandler<GetUserByIdRequest, ErrorOr<UserDTO>>
{
    public async Task<ErrorOr<UserDTO>> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null)
        {
            return Error.NotFound();
        }

        var userDTO = _mapper.Map<UserDTO>(user);

        return userDTO;
    }
}
