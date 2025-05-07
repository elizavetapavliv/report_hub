using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using MediatR;

namespace Exadel.ReportHub.Handlers.User.GetProfile;

public record GetUsersProfilesRequest(bool? IsActive) : IRequest<ErrorOr<IList<UserProfileDTO>>>;

public class GetUsersProfilesHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUsersProfilesRequest, ErrorOr<IList<UserProfileDTO>>>
{
    public async Task<ErrorOr<IList<UserProfileDTO>>> Handle(GetUsersProfilesRequest request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAsync(request.IsActive, cancellationToken);
        var userProfileDto = mapper.Map<List<UserProfileDTO>>(users);
        return userProfileDto;
    }
}
