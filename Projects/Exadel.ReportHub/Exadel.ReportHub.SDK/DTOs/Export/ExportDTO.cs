using Exadel.ReportHub.SDK.Enums;

namespace Exadel.ReportHub.SDK.DTOs.Export;

public class ExportDTO
{
    public Guid ClientId { get; set; }

    public ExportFormat Format { get; set; }
}
