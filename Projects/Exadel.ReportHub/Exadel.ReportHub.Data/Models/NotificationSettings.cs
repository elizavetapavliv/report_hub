using Exadel.ReportHub.Data.Enums;

namespace Exadel.ReportHub.Data.Models;

public class NotificationSettings
{
    public NotificationFrequency NotificationFrequency { get; set; }

    public int? NotificationDayOfMonth { get; set; }

    public DayOfWeek? NotificationDayOfWeek { get; set; }

    public int NotificationHour { get; set; }

    public ExportFormat ReportFormat { get; set; }
}
