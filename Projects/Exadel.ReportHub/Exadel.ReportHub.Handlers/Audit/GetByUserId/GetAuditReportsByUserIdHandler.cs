using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.AuditReport;
using MediatR;

namespace Exadel.ReportHub.Handlers.Audit.GetByUserId;

public record GetAuditReportsByUserIdRequest(Guid UserId) : IRequest<ErrorOr<IList<AuditReportDTO>>>;

public class GetAuditReportsByUserIdHandler(
    IAuditReportRepository auditReportRepository,
    IMapper mapper,
    IUserRepository userRepository) : IRequestHandler<GetAuditReportsByUserIdRequest, ErrorOr<IList<AuditReportDTO>>>
{
    public async Task<ErrorOr<IList<AuditReportDTO>>> Handle(GetAuditReportsByUserIdRequest request, CancellationToken cancellationToken)
    {
        var isExists = await userRepository.ExistsAsync(request.UserId, cancellationToken);
        if (!isExists)
        {
            return Error.NotFound();
        }

        var auditReports = await auditReportRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        return mapper.Map<List<AuditReportDTO>>(auditReports);
    }
}
