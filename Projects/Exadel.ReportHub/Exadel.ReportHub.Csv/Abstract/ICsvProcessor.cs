using Exadel.ReportHub.SDK.DTOs.Invoice;

namespace Exadel.ReportHub.Csv.Abstract;

public interface ICsvProcessor
{
    IEnumerable<CreateInvoiceDTO> ReadInvoices(Stream csvStream);
}
