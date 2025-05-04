using Exadel.ReportHub.Host.Jobs.Abstract;
using Exadel.ReportHub.Host.Services;
using Hangfire;

namespace Exadel.ReportHub.Host.Jobs;

public class OverduePaymentStatusJob : IJob
{
    public void Schedule()
    {
        RecurringJob.AddOrUpdate<InvoiceService>(
            recurringJobId: "OverduePaymentStatusUpdater",
            methodCall: s => s.UpdateOverdueInvoicesStatusAsync(),
            cronExpression: "0 0 * * *",
            options: new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });
    }
}
