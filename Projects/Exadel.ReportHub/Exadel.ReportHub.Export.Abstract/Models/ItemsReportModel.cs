namespace Exadel.ReportHub.Export.Abstract.Models;

public class ItemsReportModel : BaseReportModel
{
    public Guid MostSoldItemId { get; set; }

    public decimal AveragePrice { get; set; }

    public decimal AverageRevenue { get; set; }
}
