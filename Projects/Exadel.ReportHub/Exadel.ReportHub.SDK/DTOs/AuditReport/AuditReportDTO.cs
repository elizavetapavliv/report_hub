using Exadel.ReportHub.SDK.Enums;

namespace Exadel.ReportHub.SDK.DTOs.AuditReport;

public class AuditReportDTO
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid InvoiceId { get; set; }

    public DateTime TimeStamp { get; set; }

    public Status Status { get; set; }
}
