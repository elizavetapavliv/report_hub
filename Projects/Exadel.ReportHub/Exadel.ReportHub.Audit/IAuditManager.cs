using Exadel.ReportHub.Data.Enums;

namespace Exadel.ReportHub.Audit;

public interface IAuditManager
{
    Task LogExportAsync(Guid userId, Guid invoiceId, Status status, CancellationToken cancellationToken);
}
