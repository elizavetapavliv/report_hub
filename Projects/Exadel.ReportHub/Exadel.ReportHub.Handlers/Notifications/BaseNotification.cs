using MediatR;

namespace Exadel.ReportHub.Handlers.Notifications;

public record BaseNotification(Guid UserId, Dictionary<string, Guid> Properties, DateTime TimeStamp, string Action, bool IsSuccess) : INotification;
