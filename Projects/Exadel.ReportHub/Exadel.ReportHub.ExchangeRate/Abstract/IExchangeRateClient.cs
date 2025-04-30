using Exadel.ReportHub.Data.Models;

namespace Exadel.ReportHub.Ecb.Abstract;

public interface IExchangeRateClient
{
    Task<IList<ExchangeRate>> GetWeekByCurrencyAsync(string currency, DateTime date, CancellationToken cancellationToken);

    Task<ExchangeRate> GetByCurrencyAsync(string currency, DateTime date, CancellationToken cancellationToken);
}
