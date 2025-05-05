using CsvHelper.Configuration;
using Exadel.ReportHub.Export.Abstract.Models;

namespace Exadel.ReportHub.Csv.ClassMaps;

public static class ClassMapFactory
{
    public static ClassMap GetClassMap<TModel>(TModel model)
        where TModel : BaseReport
    {
        return model switch
        {
            InvoicesReport => new InvoicesReportMap(),
            ItemsReport => new ItemsReportModelMap(),
            _ => throw new ArgumentException($"No ClassMap for {typeof(TModel).Name}")
        };
    }
}
