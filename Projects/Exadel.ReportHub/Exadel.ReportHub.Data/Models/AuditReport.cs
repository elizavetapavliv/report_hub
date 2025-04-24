using Exadel.ReportHub.Data.Abstract;
using Exadel.ReportHub.Data.Enums;

namespace Exadel.ReportHub.Data.Models;

public class AuditReport : IDocument
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid InvoiceId { get; set; }

    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

    public Status Status { get; set; }
}
