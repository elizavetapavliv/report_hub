using System.Collections.ObjectModel;
using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using MediatR;

namespace Exadel.ReportHub.Handlers.User.GetActive;

public record GetActiveUsersRequest : IRequest<ErrorOr<List<UserDTO>>>;

public class GetActiveUsersHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetActiveUsersRequest, ErrorOr<List<UserDTO>>>
{
    public async Task<ErrorOr<List<UserDTO>>> Handle(GetActiveUsersRequest request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllActiveAsync(cancellationToken);

        var userDtos = mapper.Map<List<UserDTO>>(users);

        return userDtos;
    }
}
