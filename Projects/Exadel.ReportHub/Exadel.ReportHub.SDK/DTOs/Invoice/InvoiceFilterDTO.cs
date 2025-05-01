namespace Exadel.ReportHub.SDK.DTOs.Invoice;

public class InvoiceFilterDTO : DatesDTO
{
    public Guid? CustomerId { get; set; }
}
