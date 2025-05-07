﻿using Exadel.ReportHub.SDK.Enums;

namespace Exadel.ReportHub.SDK.DTOs.User;

public class UpdateUserNotificationSettingsDTO
{
    public NotificationFrequency Frequency { get; set; }

    public int? DayOfMonth { get; set; }

    public DayOfWeek? DayOfWeek { get; set; }

    public int? Hour { get; set; }

    public ExportFormat ExportFormat { get; set; }
}
