using MediatR;

namespace Exadel.ReportHub.Handlers.Notifications;

public record InvoiceExportedNotification(Guid UserId, Guid InvoiceId) : INotification;

public class InvoiceExportedNotificationHandler : INotificationHandler<InvoiceExportedNotification>
{
    public async Task Handle(InvoiceExportedNotification notification, CancellationToken cancellationToken)
    {
        await audit
    }
}