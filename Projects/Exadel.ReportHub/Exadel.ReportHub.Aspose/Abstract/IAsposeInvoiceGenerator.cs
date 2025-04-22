using Exadel.ReportHub.Pdf.Models;

namespace Exadel.ReportHub.Pdf.Abstract;

public interface IAsposeInvoiceGenerator
{
    Task<Stream> GenerateAsync(InvoiceModel invoice, CancellationToken cancellationToken);
}
