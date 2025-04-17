using Exadel.ReportHub.Handlers.ExchangeRate.Update;
using MediatR;

namespace Exadel.ReportHub.Host.Services;

public class ExchangeRateService(ISender sender)
{
    public async Task UpdateExchangeRatesAsync()
    {
        await sender.Send(new UpdateExchangeRatesRequest());
    }
}
