using Exadel.ReportHub.Ecb.Abstract;
using Exadel.ReportHub.RA.Abstract;
using MongoDB.Driver;

namespace Exadel.ReportHub.Ecb;

public class ExchangeRateProvider : IExchangeRateClient
{
    private readonly IExchangeRateRepository _exhangerRateRepository;
    private readonly IExchangeRateClient _exchangeRateProvider;

    public ExchangeRateProvider(IExchangeRateRepository exhangeRateRepository, IExchangeRateClient exchangeRateProvider)
    {
        _exhangerRateRepository = exhangeRateRepository;
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<Data.Models.ExchangeRate> GetByCurrencyAsync(string currency, CancellationToken cancellationToken)
    {
        var exchangeRate = await _exhangerRateRepository.GetByCurrencyAsync(currency, cancellationToken);

        if (exchangeRate != null)
        {
            return exchangeRate;
        }

        var exchangeRates = await GetDailyRatesAsync(cancellationToken);
        await _exhangerRateRepository.AddManyAsync(exchangeRates, cancellationToken);

        return exchangeRates.SingleOrDefault(x => x.Currency.Equals(currency, StringComparison.Ordinal));
    }

    public async Task<IList<Data.Models.ExchangeRate>> GetDailyRatesAsync(CancellationToken cancellationToken)
    {
        return await _exchangeRateProvider.GetDailyRatesAsync(cancellationToken);
    }
}
