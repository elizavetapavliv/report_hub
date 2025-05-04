namespace Exadel.ReportHub.Export.Abstract.Models;

public class InvoiceReportModel
{
    public int TotalCount { get; set; }

    public int AverageMonthCount { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal AverageAmount { get; set; }

    public int UnpaidCount { get; set; }

    public int PaidCount { get; set; }

    public DateTime ReportDate { get; set; }
}
