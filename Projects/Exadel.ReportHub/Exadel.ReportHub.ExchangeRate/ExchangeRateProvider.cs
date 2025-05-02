using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.Ecb.Abstract;
using Exadel.ReportHub.Ecb.Extensions;
using Exadel.ReportHub.RA.Abstract;

namespace Exadel.ReportHub.Ecb;

public class ExchangeRateProvider(IExchangeRateRepository exhangeRateRepository, IExchangeRateClient exchangeRateClient) : IExchangeRateProvider
{
    public async Task<ExchangeRate> GetByCurrencyForWeekAsync(string currency, DateTime weekEndDate, CancellationToken cancellationToken)
    {
        var exchangeRate = await exhangeRateRepository.GetByCurrencyAsync(currency, weekEndDate, cancellationToken);

        if (exchangeRate != null)
        {
            return exchangeRate;
        }

        for (int i = 0; i < Constants.WeeksLimit; i++)
        {
            var exchangeRates = await exchangeRateClient.GetByCurrencyInPeriodAsync(currency, weekEndDate.GetWeekPeriodStart(), weekEndDate, cancellationToken);

            if (exchangeRates.Any())
            {
                await exhangeRateRepository.UpsertManyAsync(exchangeRates, cancellationToken);
                return exchangeRates.MaxBy(x => x.RateDate);
            }

            weekEndDate = weekEndDate.GetWeekPeriodStart().AddDays(-1);
        }

        return null;
    }
}
