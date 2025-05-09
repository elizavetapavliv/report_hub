using System.ComponentModel.DataAnnotations;
using Exadel.ReportHub.SDK.Enums;

namespace Exadel.ReportHub.SDK.DTOs.Report;

public class ExportReportDTO
{
    [Required]
    public Guid ClientId { get; set; }

    [Required]
    public ExportFormat Format { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
