namespace Exadel.ReportHub.SDK.DTOs.Invoice;

public class InvoiceFilterDTO
{
    public Guid? CustomerId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
