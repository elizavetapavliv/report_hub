namespace Exadel.ReportHub.SDK.DTOs.Invoice;

public class OverdueInvoicesResultDTO
{
    public int Count { get; set; }

    public decimal Amount { get; set; }

    public string CurrencyCode { get; set; }
}
