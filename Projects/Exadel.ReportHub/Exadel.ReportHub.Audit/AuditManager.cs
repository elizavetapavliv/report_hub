using Exadel.ReportHub.Audit.Abstract;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;

namespace Exadel.ReportHub.Audit;

public class AuditManager(IAuditReportRepository auditReportRepository) : IAuditManager
{
    public Task AuditAsync(IAuditAction auditAction, CancellationToken cancellationToken)
    {
        var auditReport = new AuditReport
        {
            UserId = auditAction.UserId,
            Properties = auditAction.Properties,
            TimeStamp = auditAction.TimeStamp,
            Action = auditAction.Action,
            IsSuccess = auditAction.IsSuccess,
        };
        return auditReportRepository.AddAsync(auditReport, cancellationToken);
    }
}
