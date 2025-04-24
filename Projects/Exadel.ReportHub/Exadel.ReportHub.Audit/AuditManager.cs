using Exadel.ReportHub.Data.Enums;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;

namespace Exadel.ReportHub.Audit;

public class AuditManager : IAuditManager
{
    private readonly IAuditReportRepository _auditRepository;
    public AuditManager(IAuditReportRepository auditRepository)
    {
        _auditRepository = auditRepository;
    }

    public Task LogExportAsync(Guid userId, Guid invoiceId, Status status, CancellationToken cancellationToken)
    {
        var auditReport = new AuditReport
        {
            UserId = userId,
            InvoiceId = invoiceId,
            Status = status
        };
        return _auditRepository.LogAsync(auditReport, cancellationToken);
    }
}
