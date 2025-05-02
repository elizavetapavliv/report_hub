namespace Exadel.ReportHub.Ecb.Extensions;

public static class DateTimeExtensions
{
    public static DateTime GetWeekPeriodStart(this DateTime endDate)
    {
        var dayCount = 6;
        return endDate.AddDays(-dayCount);
    }
}
