using System.Globalization;
using CsvHelper.Configuration;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Models;

namespace Exadel.ReportHub.Csv.ClassMaps;

public class InvoiceReportMap : ClassMap<InvoiceReportModel>
{
    public InvoiceReportMap()
    {
        AutoMap(CultureInfo.InvariantCulture);

        Map(x => x.ReportDate).TypeConverterOption.Format(Constants.DateFormat);
    }
}
