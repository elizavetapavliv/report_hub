using System.Globalization;
using CsvHelper;
using Exadel.ReportHub.Csv.ClassMaps;
using Exadel.ReportHub.Export.Abstract;

namespace Exadel.ReportHub.Csv;

public class CsvExporter : IExportStrategy
{
    public Task<bool> SatisfyAsync(ExportFormat format, CancellationToken cancellationToken)
    {
        return Task.FromResult(format == ExportFormat.Csv);
    }

    public async Task<Stream> ExportAsync<TModel>(TModel exportModel, CancellationToken cancellationToken)
    {
        var csvStream = new MemoryStream();
        await using (var writer = new StreamWriter(csvStream, leaveOpen: true))
        {
            await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap(ClassMapFactory.GetClassMap<TModel>());
            csv.WriteHeader<TModel>();
            await csv.NextRecordAsync();
            csv.WriteRecord(exportModel);
        }

        csvStream.Seek(0, SeekOrigin.Begin);
        return csvStream;
    }
}
