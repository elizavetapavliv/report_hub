using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.GetTotalNumber;

public record GetInvoicesNumberRequest(InvoiceIssueDateFilterDTO InvoiceIssueDateFilterDto, Guid ClientId, Guid CustomerId) : IRequest<ErrorOr<InvoicesNumberResultDTO>>;

public class GetInvoicesNumberHandler(IInvoiceRepository invoiceRepository) : IRequestHandler<GetInvoicesNumberRequest, ErrorOr<InvoicesNumberResultDTO>>
{
    public async Task<ErrorOr<InvoicesNumberResultDTO>> Handle(GetInvoicesNumberRequest request, CancellationToken cancellationToken)
    {
        var invoices = await invoiceRepository.GetByDateRangeAsync(
            request.InvoiceIssueDateFilterDto.StartDate,
            request.InvoiceIssueDateFilterDto.EndDate,
            request.ClientId,
            request.CustomerId,
            cancellationToken);

        return new InvoicesNumberResultDTO { InvoicesNumber = invoices.Count };
    }
}
