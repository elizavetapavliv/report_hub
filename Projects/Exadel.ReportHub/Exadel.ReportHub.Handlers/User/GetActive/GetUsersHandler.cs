﻿using System.Collections.ObjectModel;
using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using MediatR;

namespace Exadel.ReportHub.Handlers.User.GetActive;

public record GetUsersRequest(bool? IsActive) : IRequest<ErrorOr<IEnumerable<UserDTO>>>;

public class GetUsersHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUsersRequest, ErrorOr<IEnumerable<UserDTO>>>
{
    public async Task<ErrorOr<IEnumerable<UserDTO>>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAsync(request.IsActive, cancellationToken);

        var userDtos = mapper.Map<List<UserDTO>>(users);

        return userDtos;
    }
}
