using Exadel.ReportHub.SDK.DTOs.Date;

namespace Exadel.ReportHub.SDK.DTOs.Invoice;

public class InvoiceFilterDTO : DatesDTO
{
    public Guid? CustomerId { get; set; }
}
