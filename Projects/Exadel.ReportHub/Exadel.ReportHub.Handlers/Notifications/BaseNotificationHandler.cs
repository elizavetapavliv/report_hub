using Exadel.ReportHub.Audit;
using Exadel.ReportHub.Audit.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.Notifications.Invoice.Export;

public class BaseNotificationHandler(IAuditManager auditManager) : INotificationHandler<BaseNotification>
{
    public async Task Handle(BaseNotification notification, CancellationToken cancellationToken)
    {
        var action = new AuditAction(
            userId: notification.UserId,
            properties: notification.Properties,
            timeStamp: notification.TimeStamp,
            action: notification.Action,
            isSuccess: notification.IsSuccess);

        await auditManager.AuditAsync(action, cancellationToken);
    }
}
