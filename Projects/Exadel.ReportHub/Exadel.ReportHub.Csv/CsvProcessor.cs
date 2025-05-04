using System.Globalization;
using CsvHelper;
using Exadel.ReportHub.Csv.Abstract;
using Exadel.ReportHub.Csv.ClassMaps;
using Exadel.ReportHub.Csv.Infrastructure;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;

namespace Exadel.ReportHub.Csv;

public class CsvProcessor : ICsvProcessor, IExportStrategy
{
    public IList<CreateInvoiceDTO> ReadInvoices(Stream csvStream)
    {
        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<CreateInvoiceMap>();

        return csv.GetRecords<CreateInvoiceDTO>().ToList();
    }

    public async Task<Stream> GenerateAsync<TModel>(TModel exportModel, CancellationToken cancellationToken)
    {
        var csvStream = new MemoryStream();
        await using (var writer = new StreamWriter(csvStream, leaveOpen: true))
        {
            await using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap(ClassMapFactory.GetClassMap(typeof(TModel)));
                csv.WriteHeader<TModel>();
                await csv.NextRecordAsync();
                csv.WriteRecord(exportModel);
            }

            await writer.FlushAsync(cancellationToken);
        }

        csvStream.Seek(0, SeekOrigin.Begin);
        return csvStream;
    }
}
