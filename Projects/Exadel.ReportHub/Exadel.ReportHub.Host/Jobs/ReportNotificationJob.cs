using Exadel.ReportHub.Handlers.Report.Send;
using Exadel.ReportHub.Host.Jobs.Abstract;
using Hangfire;
using MediatR;

namespace Exadel.ReportHub.Host.Jobs;

public class ReportNotificationJob(ISender sender) : IJob
{
    public void Schedule()
    {
        RecurringJob.AddOrUpdate<ReportNotificationJob>(
            recurringJobId: Constants.Job.Id.SendUserReports,
            methodCall: j => j.ExecuteAsync(),
            cronExpression: "0 * * * *",
            options: new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc
            });
    }

    public async Task ExecuteAsync()
    {
            var usersToNotify = await sender.Send(new SendReportsRequest());
    }
}
