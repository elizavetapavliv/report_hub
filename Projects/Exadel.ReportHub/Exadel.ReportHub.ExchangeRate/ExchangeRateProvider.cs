using Exadel.ReportHub.Ecb.Abstract;
using Exadel.ReportHub.RA.Abstract;

namespace Exadel.ReportHub.Ecb;

public class ExchangeRateProvider(IExchangeRateRepository exhangeRateRepository, IExchangeRateClient exchangeRateClient) : IExchangeRateClient
{
    public async Task<Data.Models.ExchangeRate> GetByCurrencyAsync(string currency, DateTime date, CancellationToken cancellationToken)
    {
        var exchangeRate = await exhangeRateRepository.GetByCurrencyAsync(currency, date, cancellationToken);

        if (exchangeRate != null)
        {
            return exchangeRate;
        }

        var exchangeRates = await GetRatesAsync(currency, date, cancellationToken);
        return exchangeRates.OrderByDescending(x => x.RateDate).FirstOrDefault();
    }

    public async Task<IList<Data.Models.ExchangeRate>> GetWeekByCurrencyAsync(string currency, DateTime date, CancellationToken cancellationToken)
    {
        var exchangeRates = await exhangeRateRepository.GetWeekByCurrencyAsync(currency, date, cancellationToken);

        if (exchangeRates.Any())
        {
            return exchangeRates;
        }

        return await GetRatesAsync(currency, date, cancellationToken);
    }

    private async Task<IList<Data.Models.ExchangeRate>> GetRatesAsync(string currency, DateTime date, CancellationToken cancellationToken)
    {
        var daysCount = 7;
        while (true)
        {
            var exchangeRates = await exchangeRateClient.GetWeekByCurrencyAsync(currency, date, cancellationToken);

            if (exchangeRates.Any())
            {
                await exhangeRateRepository.AddManyAsync(exchangeRates, cancellationToken);
                return exchangeRates;
            }

            date = date.AddDays(-daysCount);
        }
    }
}
