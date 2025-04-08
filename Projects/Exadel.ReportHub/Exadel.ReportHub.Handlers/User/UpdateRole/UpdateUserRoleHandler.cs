using ErrorOr;
using Exadel.ReportHub.Data.Enums;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.User.UpdateRole;

public record UpdateUserRoleRequest(Guid Id, Guid clientId, UserRole Role) : IRequest<ErrorOr<Updated>>;

public class UpdateUserRoleHandler(IUserAssignmentRepository userAssignmentRepository) : IRequestHandler<UpdateUserRoleRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateUserRoleRequest request, CancellationToken cancellationToken)
    {
        if (!await userAssignmentRepository.ExistsAsync(request.Id, request.clientId, cancellationToken))
        {
            return Error.NotFound();
        }

        await userAssignmentRepository.UpdateRoleAsync(request.Id, request.clientId, request.Role, cancellationToken);
        return Result.Updated;
    }
}
