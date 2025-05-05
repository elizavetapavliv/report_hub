using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.AuditReport;
using Exadel.ReportHub.SDK.DTOs.Pagination;
using MediatR;

namespace Exadel.ReportHub.Handlers.Audit.GetByUserId;

public record GetAuditReportsByUserIdRequest(Guid UserId, PageRequestDTO PageRequestDto) : IRequest<ErrorOr<IList<AuditReportDTO>>>;

public class GetAuditReportsByUserIdHandler(
    IAuditReportRepository auditReportRepository,
    IMapper mapper) : IRequestHandler<GetAuditReportsByUserIdRequest, ErrorOr<IList<AuditReportDTO>>>
{
    public async Task<ErrorOr<IList<AuditReportDTO>>> Handle(GetAuditReportsByUserIdRequest request, CancellationToken cancellationToken)
    {
        request.PageRequestDto.Top = request.PageRequestDto.Top == 0 ?
            Constants.Validation.Pagination.DefaultMaxValue : request.PageRequestDto.Top;

        var auditReports = await auditReportRepository.GetByUserIdAsync(request.UserId,
            request.PageRequestDto.Skip, request.PageRequestDto.Top, cancellationToken);
        return mapper.Map<List<AuditReportDTO>>(auditReports);
    }
}
