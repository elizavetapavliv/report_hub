using ErrorOr;
using Exadel.ReportHub.Ecb.Abstract;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.GetTotalRevenue;

public record GetInvoicesTotalRevenueRequest(InvoiceIssueDateFilterDTO InvoiceDateFilterDto, Guid ClientId) : IRequest<ErrorOr<TotalInvoicesRevenueDTO>>;

public class GetInvoicesTotalRevenueHandler(
    IInvoiceRepository invoiceRepository,
    ICurrencyConverter currencyConverter,
    IClientRepository clientRepository) : IRequestHandler<GetInvoicesTotalRevenueRequest, ErrorOr<TotalInvoicesRevenueDTO>>
{
    public async Task<ErrorOr<TotalInvoicesRevenueDTO>> Handle(GetInvoicesTotalRevenueRequest request, CancellationToken cancellationToken)
    {
        var sumOfInvoicesAmount = await invoiceRepository.GetByDateRangeAsync(request.ClientId, request.InvoiceDateFilterDto.StartDate, request.InvoiceDateFilterDto.EndDate, cancellationToken);

        var client = await clientRepository.GetByIdAsync(request.ClientId, cancellationToken);

        var convertedAmounts = await Task.WhenAll(sumOfInvoicesAmount
            .Select(x => currencyConverter.ConvertAsync(x.Value, x.Key,
            client.CurrencyCode, cancellationToken)).ToList());

        var totalRevenue = convertedAmounts.Sum();
        TotalInvoicesRevenueDTO totalRevenueResult = new()
        {
            TotalRevenue = totalRevenue,
            CurrencyCode = client.CurrencyCode
        };
        return totalRevenueResult;
    }
}
