using Exadel.ReportHub.Ecb.Abstract;
using Exadel.ReportHub.RA.Abstract;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Exadel.ReportHub.Ecb;

public class ExchangeRateProvider(IExchangeRateRepository exhangeRateRepository, IExchangeRateClient exchangeRateClient,
    ILogger<ExchangeRateProvider> logger) : IExchangeRateClient
{
    public async Task<Data.Models.ExchangeRate> GetByCurrencyAsync(string currency, CancellationToken cancellationToken)
    {
        var exchangeRate = await exhangeRateRepository.GetByCurrencyAsync(currency, cancellationToken);

        if (exchangeRate != null)
        {
            return exchangeRate;
        }

        var exchangeRates = await TryGetRatesAsync(cancellationToken);
        return exchangeRates?.SingleOrDefault(x => x.Currency.Equals(currency, StringComparison.Ordinal));
    }

    public async Task<IList<Data.Models.ExchangeRate>> GetDailyRatesAsync(CancellationToken cancellationToken)
    {
        var exchangeRates = await exhangeRateRepository.GetAllAsync(cancellationToken);

        if (exchangeRates.Any())
        {
            return exchangeRates;
        }

        return await TryGetRatesAsync(cancellationToken);
    }

    private async Task<IList<Data.Models.ExchangeRate>> TryGetRatesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var exchangeRates = await exchangeRateClient.GetDailyRatesAsync(cancellationToken);
            if (!exchangeRates.Any())
            {
                logger.LogError(Constants.Error.ExchangeRate.EcbReturnsNothing);
            }
            else
            {
                await exhangeRateRepository.AddManyAsync(exchangeRates, cancellationToken);
                return exchangeRates;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, Constants.Error.ExchangeRate.RatesUpdateError);
        }

        return null;
    }
}
