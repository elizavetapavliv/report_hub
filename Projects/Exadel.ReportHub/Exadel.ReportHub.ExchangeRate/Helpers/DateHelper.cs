namespace Exadel.ReportHub.Ecb.Helpers;

public static class DateHelper
{
    public static DateTime GetWeekPeriodStart(this DateTime endDate)
    {
        var dayCount = 6;
        return endDate.AddDays(-dayCount);
    }
}
