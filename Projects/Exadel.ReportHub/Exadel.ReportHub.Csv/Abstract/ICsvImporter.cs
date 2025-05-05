using Exadel.ReportHub.SDK.DTOs.Invoice;

namespace Exadel.ReportHub.Csv.Abstract;

public interface ICsvImporter
{
    IList<CreateInvoiceDTO> ReadInvoices(Stream csvStream);
}
