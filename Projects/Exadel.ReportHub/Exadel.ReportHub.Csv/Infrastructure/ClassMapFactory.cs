using CsvHelper.Configuration;
using Exadel.ReportHub.Csv.ClassMaps;
using Exadel.ReportHub.Export.Abstract.Models;

namespace Exadel.ReportHub.Csv.Infrastructure;

public static class ClassMapFactory
{
    public static ClassMap GetClassMap(Type modelType)
    {
        if (modelType == typeof(InvoicesReportModel))
        {
            return new InvoicesReportMap();
        }

        if (modelType == typeof(ItemsReportModel))
        {
            return new ItemsReportModelMap();
        }

        throw new ArgumentException($"No ClassMap for {modelType.Name}");
    }
}
