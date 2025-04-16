using Exadel.ReportHub.Data.Models;

namespace Exadel.ReportHub.Common.Providers;

public interface IExchangeRateProvider
{
    Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync();
}
