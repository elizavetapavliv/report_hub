using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using Exadel.ReportHub.SDK.Enums;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.GetNumber;

public record GetInvoicesNumberRequest(InvoiceFilterDTO InvoiceFilterDTO, Guid ClientId) : IRequest<ErrorOr<InvoicesNumberResultDTO>>;

public class GetInvoicesNumberHandler(IInvoiceRepository invoiceRepository) : IRequestHandler<GetInvoicesNumberRequest, ErrorOr<InvoicesNumberResultDTO>>
{
    public async Task<ErrorOr<InvoicesNumberResultDTO>> Handle(GetInvoicesNumberRequest request, CancellationToken cancellationToken)
    {
        var invoicesNumber = await invoiceRepository.GetCountByDateRangeAsync(
            request.InvoiceFilterDTO.StartDate,
            request.InvoiceFilterDTO.EndDate,
            request.ClientId,
            request.InvoiceFilterDTO.CustomerId,
            cancellationToken);

        var importer = request.InvoiceFilterDTO.CustomerId is null
            ? ImporterType.Client
            : ImporterType.Customer;

        return new InvoicesNumberResultDTO { InvoicesNumber = invoicesNumber, ImportedBy = importer };
    }
}
