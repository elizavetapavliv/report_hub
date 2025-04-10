using Exadel.ReportHub.SDK.DTOs.Invoice;

namespace Exadel.ReportHub.Csv.Abstracts;

public interface ICsvProcessor
{
    IEnumerable<CreateInvoiceDTO> ReadInvoices(Stream csvStream);
}
