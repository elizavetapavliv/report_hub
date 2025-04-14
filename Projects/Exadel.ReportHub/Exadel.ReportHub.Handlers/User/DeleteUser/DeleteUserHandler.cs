using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.User.DeleteUser;

public record DeleteUserRequest(Guid UserId) : IRequest<ErrorOr<Deleted>>;

public class DeleteUserHandler(IUserRepository userRepository, IUserAssignmentRepository userAssignmentRepository) : IRequestHandler<DeleteUserRequest, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var userExists = await userRepository.ExistsAsync(request.UserId, cancellationToken);
        if (!userExists)
        {
            return Error.NotFound();
        }

        var clientIds = await userAssignmentRepository.GetClientIdsByUserIdAsync(request.UserId, cancellationToken);

        await userRepository.DeleteUserAsync(request.UserId, cancellationToken);

        if(clientIds != null)
        {
            await userAssignmentRepository.DeleteUserAssignmentAsync(request.UserId, clientIds, cancellationToken);
        }

        return Result.Deleted;
    }
}
