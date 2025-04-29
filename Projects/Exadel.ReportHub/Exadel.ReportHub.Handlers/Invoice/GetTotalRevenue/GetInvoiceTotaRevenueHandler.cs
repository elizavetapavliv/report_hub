using ErrorOr;
using Exadel.ReportHub.Ecb.Abstract;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.GetTotalRevenue;

public record GetInvoicesTotalRevenueRequest(InvoiceIssueDateFilterDTO InvoiceDateFilterDto, Guid ClientId) : IRequest<ErrorOr<TotalInvoicesRevenueResult>>;

public class GetInvoicesTotalRevenueHandler(
    IInvoiceRepository invoiceRepository,
    ICurrencyConverter currencyConverter,
    IClientRepository clientRepository) : IRequestHandler<GetInvoicesTotalRevenueRequest, ErrorOr<TotalInvoicesRevenueResult>>
{
    public async Task<ErrorOr<TotalInvoicesRevenueResult>> Handle(GetInvoicesTotalRevenueRequest request, CancellationToken cancellationToken)
    {
        var invoices = await invoiceRepository.GetByDateRangeAsync(request.InvoiceDateFilterDto.StartDate, request.InvoiceDateFilterDto.EndDate, cancellationToken);

        var client = await clientRepository.GetByIdAsync(request.ClientId, cancellationToken);

        var convertedAmounts = await Task.WhenAll(invoices
            .Select(invoice => currencyConverter.ConvertAsync(invoice.Amount, invoice.CurrencyCode,
            client.CurrencyCode, cancellationToken)).ToList());

        var totalRevenue = convertedAmounts.Sum();
        TotalInvoicesRevenueResult totalRevenueResult = new()
        {
            SumOfInvoices = invoices.Count,
            TotalRevenue = totalRevenue,
            CurrencyCode = client.CurrencyCode
        };
        return totalRevenueResult;
    }
}
