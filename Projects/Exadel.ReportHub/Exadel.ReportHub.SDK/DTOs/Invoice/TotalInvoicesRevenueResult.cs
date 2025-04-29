namespace Exadel.ReportHub.SDK.DTOs.Invoice;

public class TotalInvoicesRevenueResult
{
    public int SumOfInvoices { get; set; }

    public decimal TotalRevenue { get; set; }

    public string CurrencyCode { get; set; }
}
