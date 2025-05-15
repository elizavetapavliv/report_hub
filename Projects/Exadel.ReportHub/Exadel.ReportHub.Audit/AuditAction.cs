using Exadel.ReportHub.Audit.Abstract;

namespace Exadel.ReportHub.Audit;

public class AuditAction(Guid userId, Dictionary<string, Guid> properties, DateTime timeStamp, string action, bool isSuccess) : IAuditAction
{
    public Guid UserId { get; } = userId;

    public Dictionary<string, Guid> Properties { get; } = properties;

    public DateTime TimeStamp { get; } = timeStamp;

    public string Action { get; } = action;

    public bool IsSuccess { get; } = isSuccess;
}
