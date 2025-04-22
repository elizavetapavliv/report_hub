using Exadel.ReportHub.Ecb;
using Exadel.ReportHub.Handlers.Invoice.Import.Decorator_replace;
using Exadel.ReportHub.RA.Abstract;

namespace Exadel.ReportHub.Handlers.Invoice.Import.CurrencyConverter_replace;

public class CurrencyConverter : ICurrencyConverter
{
    private readonly IExchangeRateRepository _exchangeRateRepository;

    public CurrencyConverter(IExchangeRateRepository exchangeRateRepository, IExchangeRateProvider exchangeRateProvider)
    {
        _exchangeRateRepository = new ExchangeRateDecorator(exchangeRateRepository, exchangeRateProvider);
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
            fromRate = (await _exchangeRateRepository.GetByCurrencyAsync(fromCurrency, cancellationToken)).Rate;
        }

        decimal toRate = 1;
        if (!toCurrency.Equals("EUR", StringComparison.Ordinal))
        {
            toRate = (await _exchangeRateRepository.GetByCurrencyAsync(toCurrency, cancellationToken)).Rate;
        }

        return amount * toRate / fromRate;
    }
}
