using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.GetRevenue;

public record GetInvoicesRevenueRequest(InvoiceRevenueFilterDTO InvoiceRevenueFilterDto) : IRequest<ErrorOr<TotalInvoicesRevenueDTO>>;

public class GetInvoicesRevenueHandler(IInvoiceRepository invoiceRepository) : IRequestHandler<GetInvoicesRevenueRequest, ErrorOr<TotalInvoicesRevenueDTO>>
{
    public async Task<ErrorOr<TotalInvoicesRevenueDTO>> Handle(GetInvoicesRevenueRequest request, CancellationToken cancellationToken)
    {
        var result = await invoiceRepository.GetTotalAmountByDateRangeAsync(request.InvoiceRevenueFilterDto.ClientId,
            request.InvoiceRevenueFilterDto.StartDate, request.InvoiceRevenueFilterDto.EndDate, cancellationToken);

        if (!result.HasValue)
        {
            return Error.NotFound(nameof(Constants.Error.Invoice.NotFoundInSelectedPeriod), Constants.Error.Invoice.NotFoundInSelectedPeriod);
        }

        var (currencyCode, total) = result.Value;

        return new TotalInvoicesRevenueDTO
        {
            TotalRevenue = total,
            CurrencyCode = currencyCode
        };
    }
}
