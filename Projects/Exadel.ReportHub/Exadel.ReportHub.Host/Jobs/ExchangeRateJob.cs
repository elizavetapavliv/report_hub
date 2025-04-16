using Exadel.ReportHub.Handlers.ExchangeRate.Update;
using MediatR;

namespace Exadel.ReportHub.Host.Jobs;

public class ExchangeRateJob(ISender sender)
{
    public async Task UpdateExchangeRatesAsync()
    {
        await sender.Send(new UpdateExchangeRatesRequest());
    }
}
