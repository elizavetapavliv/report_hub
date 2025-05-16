using Exadel.ReportHub.Audit.Abstract;

namespace Exadel.ReportHub.Audit;

public class ExportPlansReportAuditAction : IAuditAction
{
    public Guid UserId { get; }

    public Dictionary<string, Guid> Properties { get; }

    public DateTime TimeStamp { get; }

    public string Action { get; }

    public bool IsSuccess { get; }

    public ExportPlansReportAuditAction(Guid userId, Guid clientId, DateTime timeStamp, bool isSuccess)
    {
        UserId = userId;
        Properties = new Dictionary<string, Guid>
        {
            { "ClientId", clientId }
        };
        TimeStamp = timeStamp;
        Action = nameof(ExportPlansReportAuditAction);
        IsSuccess = isSuccess;
    }
}
