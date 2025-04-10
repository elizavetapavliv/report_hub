using Exadel.ReportHub.Data.Models;

namespace Exadel.ReportHub.RA.Abstract;

public interface IInvoiceRepository
{
    Task ImportAsync(IEnumerable<Invoice> invoices, CancellationToken cancellationToken);
}
