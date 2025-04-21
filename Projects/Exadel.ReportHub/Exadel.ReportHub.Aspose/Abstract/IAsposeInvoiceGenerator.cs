using Exadel.ReportHub.SDK.DTOs.Invoice;

namespace Exadel.ReportHub.Aspose.Abstract;

public interface IAsposeInvoiceGenerator
{
    MemoryStream Generate(GenerateInvoiceDTO invoiceDto);
}
