using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.UserDTOs;
using MediatR;

namespace Exadel.ReportHub.Handlers.UserHandlers;

public record GetAllUsersRequest : IRequest<ErrorOr<IEnumerable<GetUserDTO>>>;

public class GetAllUsersHandler(IUserRepository userRepository) : IRequestHandler<GetAllUsersRequest, ErrorOr<IEnumerable<GetUserDTO>>>
{
    public async Task<ErrorOr<IEnumerable<GetUserDTO>>> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllUsersAsync(cancellationToken);
        var usersDTO = users.Select(u => new GetUserDTO
        {
            Id = u.Id,
            Email = u.Email,
            FullName = u.FullName
        }).ToList();

        return await Task.FromResult<ErrorOr<IEnumerable<GetUserDTO>>>(usersDTO);
    }
}
