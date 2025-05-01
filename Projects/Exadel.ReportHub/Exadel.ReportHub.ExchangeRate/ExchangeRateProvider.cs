using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.Ecb.Abstract;
using Exadel.ReportHub.Ecb.Helpers;
using Exadel.ReportHub.RA.Abstract;

namespace Exadel.ReportHub.Ecb;

public class ExchangeRateProvider(IExchangeRateRepository exhangeRateRepository, IExchangeRateClient exchangeRateClient) : IExchangeRateProvider
{
    public async Task<ExchangeRate> GetByCurrencyForWeekAsync(string currency, DateTime date, CancellationToken cancellationToken)
    {
        var exchangeRate = await exhangeRateRepository.GetByCurrencyAsync(currency, date, cancellationToken);

        if (exchangeRate != null)
        {
            return exchangeRate;
        }

        var limit = 4;
        while (limit > 0)
        {
            var exchangeRates = await exchangeRateClient.GetByCurrencyInPeriodAsync(currency, date.GetWeekPeriodStart(), date, cancellationToken);

            if (exchangeRates.Any())
            {
                await exhangeRateRepository.AddManyAsync(exchangeRates, cancellationToken);
                return exchangeRates.MaxBy(x => x.RateDate);
            }

            date = date.GetWeekPeriodStart().AddDays(-1);
            limit++;
        }

        return null;
    }
}
