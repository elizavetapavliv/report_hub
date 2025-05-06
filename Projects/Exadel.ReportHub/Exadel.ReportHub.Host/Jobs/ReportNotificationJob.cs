using Exadel.ReportHub.Data.Enums;
using Exadel.ReportHub.Handlers.User.GetByNotification;
using Exadel.ReportHub.Host.Jobs.Abstract;
using Exadel.ReportHub.RA.Abstract;
using Hangfire;
using MediatR;

namespace Exadel.ReportHub.Host.Jobs;

public class ReportNotificationJob(
    ILogger<ReportNotificationJob> logger,
    ISender sender
    ) : IJob
{
    public void Schedule()
    {
        RecurringJob.AddOrUpdate<ReportNotificationJob>(
            recurringJobId: "SendUserReportsJob",
            methodCall: j => j.ExecuteAsync(),
            cronExpression: "0 * * * *",
            options: new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc
            });
    }

    public async Task ExecuteAsync()
    {
        try
        {
            var usersToNotify = sender.Send(new GetUsersByNotificationRequest());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while sending reports at {Time}", DateTime.UtcNow);
        }
    }
}
