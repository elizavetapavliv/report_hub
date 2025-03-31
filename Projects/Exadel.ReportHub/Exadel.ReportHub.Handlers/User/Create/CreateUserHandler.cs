using System.Runtime.InteropServices;
using ErrorOr;
using Exadel.ReportHub.Common;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using MediatR;

namespace Exadel.ReportHub.Handlers.User.Create;

public record CreateUserRequest(CreateUserDto CreateUserDTO) : IRequest<ErrorOr<UserDto>>;

public class CreateUserHandler(IUserRepository userRepository) : IRequestHandler<CreateUserRequest, ErrorOr<UserDto>>
{
    public async Task<ErrorOr<UserDto>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var (passwordHash, passwordSalt) = PasswordHasher.CreatePasswordHash(request.CreateUserDTO.Password);

        var user = new Data.Models.User
        {
            Id = Guid.NewGuid(),
            Email = request.CreateUserDTO.Email,
            FullName = request.CreateUserDTO.FullName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        await userRepository.AddAsync(user, cancellationToken);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName
        };
    }
}
