using ErrorOr;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using MediatR;

namespace Exadel.ReportHub.Handlers.UserHandlers.Create;

public record CreateUserRequest(string Email, string FullName, string Password) : IRequest<ErrorOr<UserDTO>>;

public class CreateUserHandler(IUserRepository userRepository) : IRequestHandler<CreateUserRequest, ErrorOr<UserDTO>>
{
    public async Task<ErrorOr<UserDTO>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FullName = request.FullName,
            Password = request.Password
        };

        await userRepository.AddAsync(user, cancellationToken);

        return new UserDTO
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
        };
    }
}
