using Exadel.ReportHub.SDK.DTOs.Invoice;

namespace Exadel.ReportHub.Handlers.Managers;

public interface IInvoiceManager
{
    Task<IList<Data.Models.Invoice>> GenerateInvoicesAsync(IEnumerable<CreateInvoiceDTO> invoiceDtos, CancellationToken cancellationToken);
}
