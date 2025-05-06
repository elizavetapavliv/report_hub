using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Models;

namespace Exadel.ReportHub.Csv.ClassMaps;

public class PlansReportMap : BaseReportMap<PlansReport>
{
    public PlansReportMap()
    {
        Map(x => x.StartDate).TypeConverterOption.Format(Constants.Format.Date);
        Map(x => x.EndDate).TypeConverterOption.Format(Constants.Format.Date);
    }
}
