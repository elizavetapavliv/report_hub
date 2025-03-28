using ErrorOr;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.UserHandlers;

public record CreateUserRequest(string Email, string FullName, string Password) : IRequest<ErrorOr<Created>>;

public class CreateUserHandler(IUserRepository userRepository) : IRequestHandler<CreateUserRequest, ErrorOr<Created>>
{
    public async Task<ErrorOr<Created>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        if (existingUser is not null)
        {
            return await Task.FromResult<ErrorOr<Created>>(Error.Validation(description: $"User with email: '{request.Email}' is already exists."));
        }

        var user = new User
        {
            Email = request.Email,
            FullName = request.FullName,
            Password = request.Password
        };

        await userRepository.AddUserAsync(user, cancellationToken);

        return await Task.FromResult<ErrorOr<Created>>(Result.Created);
    }
}
