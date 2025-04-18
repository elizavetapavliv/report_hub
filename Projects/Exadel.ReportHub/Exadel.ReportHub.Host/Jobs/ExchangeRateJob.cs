using Exadel.ReportHub.Host.Services;
using Exadel.ReportHub.SDK.Abstract;
using Hangfire;

namespace Exadel.ReportHub.Host.Job;

public class ExchangeRateJob : IJob
{
    public void Schedule()
    {
        RecurringJob.AddOrUpdate<ExchangeRateService>(
            recurringJobId: "ExchangeRateUpdater",
            methodCall: s => s.UpdateExchangeRatesAsync(),
            cronExpression: "0 16 * * 1-5",
            options: new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });
    }
}
