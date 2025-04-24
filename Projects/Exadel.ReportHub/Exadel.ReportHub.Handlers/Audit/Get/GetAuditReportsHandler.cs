using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.AuditReport;
using MediatR;

namespace Exadel.ReportHub.Handlers.Audit.Get;

public record GetAuditReportsRequest : IRequest<ErrorOr<IList<AuditReportDTO>>>;

public class GetAuditReportsHandler(IAuditReportRepository auditReportRepository, IMapper mapper) : IRequestHandler<GetAuditReportsRequest, ErrorOr<IList<AuditReportDTO>>>
{
    public async Task<ErrorOr<IList<AuditReportDTO>>> Handle(GetAuditReportsRequest request, CancellationToken cancellationToken)
    {
        var auditReports = await auditReportRepository.GetAllAsync(cancellationToken);
        var auditReportDtos = mapper.Map<List<AuditReportDTO>>(auditReports);
        return auditReportDtos;
    }
}
