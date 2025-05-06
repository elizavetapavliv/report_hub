using Exadel.ReportHub.SDK.Enums;

namespace Exadel.ReportHub.SDK.DTOs.User;

public class UpdateUserNotificationFrequencyDTO
{
    public NotificationFrequency NotificationFrequency { get; set; }

    public int NotificationDayOfMonth { get; set; }

    public DayOfWeek? NotificationDayOfWeek { get; set; }

    public int NotificationTime { get; set; }
}
