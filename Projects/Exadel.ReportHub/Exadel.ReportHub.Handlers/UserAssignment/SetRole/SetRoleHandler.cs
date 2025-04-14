using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.UserAssignment;
using MediatR;

namespace Exadel.ReportHub.Handlers.UserAssignment.SetRole;

public record SetRoleRequest(SetUserAssignmentDTO SetUserAssignmentDTO) : IRequest<ErrorOr<Updated>>;

public class SetRoleHandler(IUserAssignmentRepository userAssignmentRepository, IMapper mapper) : IRequestHandler<SetRoleRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(SetRoleRequest request, CancellationToken cancellationToken)
    {
        var userAssignment = mapper.Map<Data.Models.UserAssignment>(request.SetUserAssignmentDTO);

        await userAssignmentRepository.SetRoleAsync(userAssignment, cancellationToken);
        return Result.Updated;
    }
}
