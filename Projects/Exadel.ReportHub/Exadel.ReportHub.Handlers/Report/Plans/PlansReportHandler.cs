using ErrorOr;
using Exadel.ReportHub.Common.Providers;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Handlers.Managers.Report;
using Exadel.ReportHub.Handlers.Notifications;
using Exadel.ReportHub.RA;
using Exadel.ReportHub.SDK.DTOs.Report;
using MediatR;

namespace Exadel.ReportHub.Handlers.Report.Plans;

public record PlansReportRequest(ExportReportDTO ExportReportDto) : IRequest<ErrorOr<ExportResult>>;

public class PlansReportHandler(IReportManager reportManager, IUserProvider userProvider, IPublisher publisher) : IRequestHandler<PlansReportRequest, ErrorOr<ExportResult>>
{
    public async Task<ErrorOr<ExportResult>> Handle(PlansReportRequest request, CancellationToken cancellationToken)
    {
        var userId = userProvider.GetUserId();
        var isSuccess = true;

        try
        {
            return await reportManager.GeneratePlansReportAsync(request.ExportReportDto, cancellationToken);
        }
        catch
        {
            isSuccess = false;

            return Error.Unexpected();
        }
        finally
        {
            var props = new Dictionary<string, Guid>
            {
                [nameof(request.ExportReportDto.ClientId)] = request.ExportReportDto.ClientId
            };

            var notification = new BaseNotification(userId, props, DateTime.UtcNow, Constants.Notification.ExportPlansReportAction, isSuccess);
            await publisher.Publish(notification, cancellationToken);
        }
    }
}
