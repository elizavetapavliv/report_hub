using Hangfire;

namespace Exadel.ReportHub.Host.Services;

public class SchedulerService
{
    public void StartJobs()
    {
        RecurringJob.AddOrUpdate<ExchangeRateService>(
            recurringJobId: "ExchangeRateUpdater",
            methodCall: job => job.UpdateExchangeRatesAsync(),
            cronExpression: "0 14 * * 1-5",
            options: new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc
            });

        RecurringJob.AddOrUpdate<PingService>(
            recurringJobId: "SelfPing",
            methodCall: ping => ping.PingAsync(),
            cronExpression: "*/14 * * * *",
            options: new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc
            });
    }
}
