namespace Exadel.ReportHub.Export.Abstract.Models;

public class InvoicesReportModel : BaseReportModel
{
    public int TotalCount { get; set; }

    public int AverageMonthCount { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal AverageAmount { get; set; }

    public int UnpaidCount { get; set; }

    public int OverdueCount { get; set; }

    public int PaidOnTimeCount { get; set; }

    public int PaidLateCount { get; set; }
}
