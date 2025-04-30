using Exadel.ReportHub.SDK.Enums;

namespace Exadel.ReportHub.SDK.DTOs.Invoice;

public class InvoicesNumberResultDTO
{
    public long InvoicesNumber { get; set; }

    public ImporterType ImportedBy { get; set; }
}
