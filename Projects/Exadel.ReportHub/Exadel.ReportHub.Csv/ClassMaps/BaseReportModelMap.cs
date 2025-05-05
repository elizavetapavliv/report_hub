using System.Globalization;
using CsvHelper.Configuration;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Models;

namespace Exadel.ReportHub.Csv.ClassMaps;

public abstract class BaseReportModelMap<TModel> : ClassMap<TModel>
    where TModel : BaseReport
{
    protected BaseReportModelMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(x => x.ReportDate).TypeConverterOption.Format(Constants.Format.Date);
    }
}
