using Exadel.ReportHub.Pdf.Models;

namespace Exadel.ReportHub.Pdf.Abstract;

public interface IPdfInvoiceGenerator
{
    Stream Generate(InvoiceModel invoice);
}
