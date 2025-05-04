using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.GetByOverdueStatus;

public record GetInvoicesByOverdueStatusRequest(Guid clientId) : IRequest<ErrorOr<OverdueInvoicesResultDTO>>;

public class GetInvoicesByOverdueStatusHandler(IInvoiceRepository invoiceRepository) : IRequestHandler<GetInvoicesByOverdueStatusRequest, ErrorOr<OverdueInvoicesResultDTO>>
{
    public async Task<ErrorOr<OverdueInvoicesResultDTO>> Handle(GetInvoicesByOverdueStatusRequest request, CancellationToken cancellationToken)
    {
        var result = await invoiceRepository.GetOverdueAsync(request.clientId, cancellationToken);

        if (!result.HasValue)
        {
            return Error.NotFound(nameof(Constants.Error.Invoice.NotFoundInSelectedPeriod), Constants.Error.Invoice.NotFoundInSelectedPeriod);
        }

        var (count, total, currencyCode) = result.Value;

        return new OverdueInvoicesResultDTO
        {
            Count = count,
            Amount = total,
            CurrencyCode = currencyCode
        };
    }
}
