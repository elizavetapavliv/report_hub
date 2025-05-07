using Exadel.ReportHub.Data.Abstract;

namespace Exadel.ReportHub.Export.Abstract.Models;

public class PlansReport : BaseReport
{
    public Guid TargetItemId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int PlannedCount { get; set; }

    public int ActualCount { get; set; }
}
