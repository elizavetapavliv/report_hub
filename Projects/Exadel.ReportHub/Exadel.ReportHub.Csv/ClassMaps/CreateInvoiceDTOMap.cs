using System.Globalization;
using CsvHelper.Configuration;
using Exadel.ReportHub.SDK.DTOs.Invoice;

namespace Exadel.ReportHub.Csv.ClassMaps;

public class CreateInvoiceDTOMap : ClassMap<CreateInvoiceDTO>
{
    public CreateInvoiceDTOMap()
    {
        AutoMap(CultureInfo.InvariantCulture);

        Map(x => x.ItemIds)
            .Convert(args => args.Row.GetField(nameof(CreateInvoiceDTO.ItemIds)).Split(";").Select(Guid.Parse).ToList());
    }
}
