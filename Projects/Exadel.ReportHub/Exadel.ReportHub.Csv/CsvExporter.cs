using System.Globalization;
using CsvHelper;
using Exadel.ReportHub.Csv.ClassMaps;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Models;

namespace Exadel.ReportHub.Csv;

public class CsvExporter : IExportStrategy
{
    public async Task<Stream> ExportAsync<TModel>(TModel exportModel, CancellationToken cancellationToken)
        where TModel : BaseReport
    {
        var csvStream = new MemoryStream();
        await using (var writer = new StreamWriter(csvStream, leaveOpen: true))
        {
            await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap(ClassMapFactory.GetClassMap(exportModel));
            csv.WriteHeader<TModel>();
            await csv.NextRecordAsync();
            csv.WriteRecord(exportModel);
        }

        csvStream.Seek(0, SeekOrigin.Begin);
        return csvStream;
    }

    public bool Satisfy(ExportFormat format) => format == ExportFormat.Csv;
}
