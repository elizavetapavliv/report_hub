using Exadel.ReportHub.Ecb.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Exadel.ReportHub.Ecb.Helpers;

public class CurrencyConverter : ICurrencyConverter
{
    private readonly IExchangeRateClient _exchangeRateService;

    public CurrencyConverter(IServiceProvider serviceProvider)
    {
        _exchangeRateService = serviceProvider.GetRequiredService<ExchangeRateProvider>();
    }

    public async Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency, CancellationToken cancellationToken)
    {
        if (fromCurrency.Equals(toCurrency, StringComparison.Ordinal))
        {
            return amount;
        }

        decimal fromRate = 1;
        if (!fromCurrency.Equals("EUR", StringComparison.Ordinal))
        {
            fromRate = (await _exchangeRateService.GetByCurrencyAsync(fromCurrency, cancellationToken)).Rate;
        }

        decimal toRate = 1;
        if (!toCurrency.Equals("EUR", StringComparison.Ordinal))
        {
            toRate = (await _exchangeRateService.GetByCurrencyAsync(toCurrency, cancellationToken)).Rate;
        }

        return amount * toRate / fromRate;
    }
}
