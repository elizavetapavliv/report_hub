using Exadel.ReportHub.Data.Models;

namespace Exadel.ReportHub.Data.Abstract;

public abstract class BaseReport
{
    public DateTime ReportDate { get; set; }

    public ChartData ChartData { get; set; }
}
