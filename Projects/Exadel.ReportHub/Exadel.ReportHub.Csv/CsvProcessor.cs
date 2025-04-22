using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Exadel.ReportHub.Csv.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;

namespace Exadel.ReportHub.Csv;

public class CsvProcessor : ICsvProcessor
{
    public IList<CreateInvoiceDTO> ReadInvoices(Stream csvStream)
    {
        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<CreateInvoiceDTOMap>();

        return csv.GetRecords<CreateInvoiceDTO>().ToList();
    }

    private sealed class CreateInvoiceDTOMap : ClassMap<CreateInvoiceDTO>
    {
        public CreateInvoiceDTOMap()
        {
            AutoMap(CultureInfo.InvariantCulture);

            Map(x => x.ItemIds)
                .Convert(args => args.Row.GetField(nameof(CreateInvoiceDTO.ItemIds)).Split(";").Select(Guid.Parse).ToList());
        }
    }
}
