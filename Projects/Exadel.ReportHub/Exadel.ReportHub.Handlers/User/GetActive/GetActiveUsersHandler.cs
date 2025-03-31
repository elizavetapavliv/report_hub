using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using MediatR;

namespace Exadel.ReportHub.Handlers.User.GetAllActive;

public record GetActiveUsersRequest : IRequest<ErrorOr<IEnumerable<UserDto>>>;

public class GetActiveUsersHandler(IUserRepository userRepository) : IRequestHandler<GetActiveUsersRequest, ErrorOr<IEnumerable<UserDto>>>
{
    public async Task<ErrorOr<IEnumerable<UserDto>>> Handle(GetActiveUsersRequest request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllActiveAsync(cancellationToken);
        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            Email = u.Email,
            FullName = u.FullName
        }).ToList();

        return userDtos;
    }
}
