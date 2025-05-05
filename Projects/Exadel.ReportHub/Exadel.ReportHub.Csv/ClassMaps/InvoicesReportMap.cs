using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Models;

namespace Exadel.ReportHub.Csv.ClassMaps;

public class InvoicesReportMap : BaseReportModelMap<InvoicesReport>
{
    public InvoicesReportMap()
    {
        Map(x => x.TotalAmount).TypeConverterOption.Format(Constants.Format.Decimal);
        Map(x => x.AverageAmount).TypeConverterOption.Format(Constants.Format.Decimal);
    }
}
