using Exadel.ReportHub.SDK.DTOs.Invoice;

namespace Exadel.ReportHub.Handlers.Managers.Invoice;

public interface IInvoiceManager
{
    Task<InvoiceDTO> GenerateInvoiceAsync(CreateInvoiceDTO createInvoiceDto, CancellationToken cancellationToken);

    Task<IList<InvoiceDTO>> GenerateInvoicesAsync(IEnumerable<CreateInvoiceDTO> createInvoiceDtos, CancellationToken cancellationToken);
}
