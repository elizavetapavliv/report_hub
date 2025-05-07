using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using MediatR;

namespace Exadel.ReportHub.Handlers.User.GetProfile;

public record GetUserProfileByIdRequest(Guid Id) : IRequest<ErrorOr<UserProfileDTO>>;

public class GetUserProfileByIdHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUserProfileByIdRequest, ErrorOr<UserProfileDTO>>
{
    public async Task<ErrorOr<UserProfileDTO>> Handle(GetUserProfileByIdRequest request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetByIdAsync(request.Id, cancellationToken);
        var userProfileDto = mapper.Map<UserProfileDTO>(users);
        return userProfileDto;
    }
}
