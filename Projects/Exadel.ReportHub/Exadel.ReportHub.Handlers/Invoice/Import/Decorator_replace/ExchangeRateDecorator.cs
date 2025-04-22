using Exadel.ReportHub.Ecb;
using Exadel.ReportHub.RA.Abstract;
using MongoDB.Driver;

namespace Exadel.ReportHub.Handlers.Invoice.Import.Decorator_replace;

public class ExchangeRateDecorator : IExchangeRateRepository
{
    private readonly IExchangeRateRepository _exhangerRateRepository;
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public ExchangeRateDecorator(IExchangeRateRepository exhangerRateRepository, IExchangeRateProvider exchangeRateProvider)
    {
        _exhangerRateRepository = exhangerRateRepository;
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task AddManyAsync(IEnumerable<Data.Models.ExchangeRate> exchangeRates, CancellationToken cancellationToken)
    {
        await _exhangerRateRepository.AddManyAsync(exchangeRates, cancellationToken);
    }

    public async Task<Data.Models.ExchangeRate> GetByCurrencyAsync(string currency, CancellationToken cancellationToken)
    {
        var exchangeRate = await _exhangerRateRepository.GetByCurrencyAsync(currency, cancellationToken);

        if (exchangeRate != null)
        {
            return exchangeRate;
        }

        exchangeRate = (await _exchangeRateProvider.GetDailyRatesAsync(cancellationToken)).SingleOrDefault(x => x.Currency.Equals(currency, StringComparison.Ordinal));

        return exchangeRate;
    }
}
