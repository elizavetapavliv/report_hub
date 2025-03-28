using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.UserDTOs;
using MediatR;

namespace Exadel.ReportHub.Handlers.UserHandlers;

public record GetUserByIdRequest(Guid Id) : IRequest<ErrorOr<GetUserDTO>>;

public class GetUserByIdHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdRequest, ErrorOr<GetUserDTO>>
{
    public async Task<ErrorOr<GetUserDTO>> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.Id, cancellationToken);
        if (user is null)
        {
            return await Task.FromResult<ErrorOr<GetUserDTO>>(Error.Validation(description: $"User with '{request.Id} id doesn't exist"));
        }

        var userDTO = new GetUserDTO
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName
        };

        return await Task.FromResult<ErrorOr<GetUserDTO>>(userDTO);
    }
}
