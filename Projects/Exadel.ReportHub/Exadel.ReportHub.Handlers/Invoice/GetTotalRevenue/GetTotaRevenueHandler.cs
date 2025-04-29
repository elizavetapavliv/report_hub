using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.Ecb.Abstract;
using Exadel.ReportHub.Handlers.Invoice.GetTotalRevenuel;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.GetTotalRevenue;

public record GetTotalRevenueRequest(InvoiceFilterDTO Filter, Guid clientId) : IRequest<ErrorOr<TotalRevenueResult>>;

public class GetTotaRevenueHandler(
    IInvoiceRepository invoiceRepository,
    ICurrencyConverter currencyConverter,
    IClientRepository clientRepository,
    IMapper mapper) : IRequestHandler<GetTotalRevenueRequest, ErrorOr<TotalRevenueResult>>
{
    public async Task<ErrorOr<TotalRevenueResult>> Handle(GetTotalRevenueRequest request, CancellationToken cancellationToken)
    {
        var map = mapper.Map<Data.Models.Invoice>(request.Filter);
        var invoices = await invoiceRepository.GetByDateRangeAsync(map, cancellationToken);
        if (invoices == null || !invoices.Any())
        {
            return Error.NotFound();
        }

        var convertedAmounts = await Task.WhenAll(invoices
            .Select(invoice => currencyConverter.ConvertAsync(invoice.Amount, invoice.CurrencyCode,
            Constants.Validation.Currency.DefaultCurrencyCode, cancellationToken)).ToList());

        var totalRevenue = convertedAmounts.Sum();
        TotalRevenueResult totalRevenueResult = new()
        {
            TotalRevenue = totalRevenue,
            CurrencyCode = Constants.Validation.Currency.DefaultCurrencyCode
        };
        return totalRevenueResult;
    }
}
