namespace Exadel.ReportHub.ExchangeRate;

public interface IExchangeRateProvider
{
    Task<IEnumerable<Data.Models.ExchangeRate>> GetDailyRatesAsync();
}
