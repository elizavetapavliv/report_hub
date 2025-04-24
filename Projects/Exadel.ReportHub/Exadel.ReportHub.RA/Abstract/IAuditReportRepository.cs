using Exadel.ReportHub.Data.Models;

namespace Exadel.ReportHub.RA.Abstract;

public interface IAuditReportRepository
{
    Task LogAsync(AuditReport auditReport, CancellationToken cancellationToken);

    Task<IList<AuditReport>> GetAllAsync(CancellationToken cancellationToken);

    Task<AuditReport> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IList<AuditReport>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
