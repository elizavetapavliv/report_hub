using System.Globalization;
using System.Net.Mail;
using Exadel.ReportHub.Email.Abstract;
using Exadel.ReportHub.Email.Models;
using Exadel.ReportHub.Export.Abstract.Helpers;
using Exadel.ReportHub.Handlers.Managers;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Report;
using Exadel.ReportHub.SDK.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Exadel.ReportHub.Handlers.Report.Send;

public record SendReportsRequest : IRequest<Unit>;

public class SendReportsHandler(IReportManager reportManager, IUserRepository userRepository, IEmailSender emailSender, ILogger<SendReportsHandler> logger)
    : IRequestHandler<SendReportsRequest, Unit>
{
    public async Task<Unit> Handle(SendReportsRequest request, CancellationToken cancellationToken)
    {
        const int daysInWeek = 7;
        const int sundayOfWeek = 7;
        var now = DateTime.UtcNow;
        var currentHour = now.Hour;
        var currentDay = now.Day;
        var currentDayOfWeek = now.DayOfWeek;
        var today = DateTime.Today;
        var users = await userRepository.GetUsersByNotificationSettingsAsync(currentDay, currentDayOfWeek, currentHour, cancellationToken);

        await Task.WhenAll(users.Select(async user =>
        {
            var (startDate, endDate) = user.NotificationSettings.ReportPeriod switch
            {
                Data.Enums.ReportPeriod.WholePeriod => (null, null),
                Data.Enums.ReportPeriod.LastMonth => (today.AddMonths(-1).AddDays(-today.Day + 1), today.AddDays(-today.Day)),
                Data.Enums.ReportPeriod.Month => (today.AddMonths(-1).AddDays(1), today),
                Data.Enums.ReportPeriod.LastWeek => today.DayOfWeek is DayOfWeek.Sunday ?
                    (today.AddDays(-(daysInWeek + sundayOfWeek - 1)), today.AddDays(-(sundayOfWeek - 1))) :
                    (today.AddDays(-(daysInWeek + (int)today.DayOfWeek - 1)), today.AddDays(-((int)today.DayOfWeek - 1))),
                Data.Enums.ReportPeriod.Week => (today.AddDays(-daysInWeek), today),
                Data.Enums.ReportPeriod.CustomPeriod => (today.AddDays(-(user.NotificationSettings.DaysCount!.Value - 1)), today),
                _ => ((DateTime?)null, (DateTime?)null)
            };

            var exportReportDto = new ExportReportDTO
            {
                ClientId = user.NotificationSettings.ClientId,
                Format = (ExportFormat)user.NotificationSettings.ExportFormat,
                StartDate = startDate,
                EndDate = endDate
            };

            var reportEmail = new ReportEmailModel
            {
                UserName = user.FullName,
                Period = user.NotificationSettings.ReportPeriod is Data.Enums.ReportPeriod.WholePeriod ?
                    "whole period" :
                    $"{FormatDate(exportReportDto.StartDate.Value)} to {FormatDate(exportReportDto.EndDate.Value)}"
            };

            var attachments = new List<Attachment>();
            try
            {
                var invoicesStreamTask = reportManager.GenerateInvoicesReportAsync(exportReportDto, cancellationToken);
                var itemsStreamTask = reportManager.GenerateItemsReportAsync(exportReportDto, cancellationToken);
                var plansStreamTask = reportManager.GeneratePlansReportAsync(exportReportDto, cancellationToken);

                await Task.WhenAll(invoicesStreamTask, itemsStreamTask, plansStreamTask);
                attachments.Add(new Attachment(invoicesStreamTask.Result,
                    $"InvoicesReport_{DateTime.Today.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                    $"{ExportFormatHelper.GetFileExtension(exportReportDto.Format)}"));
                attachments.Add(new Attachment(itemsStreamTask.Result,
                    $"ItemsReport_{DateTime.Today.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                    $"{ExportFormatHelper.GetFileExtension(exportReportDto.Format)}"));
                attachments.Add(new Attachment(plansStreamTask.Result,
                    $"PlansReport_{DateTime.Today.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                    $"{ExportFormatHelper.GetFileExtension(exportReportDto.Format)}"));
                reportEmail.IsSuccess = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to generate reports");
                reportEmail.IsSuccess = false;
            }

            try
            {
                await emailSender.SendAsync(user.Email, "Report", attachments, "Report.html", reportEmail, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send report email to user {UserId} ({Email})", user.Id, user.Email);
            }
        }));

        return Unit.Value;
    }

    private static string FormatDate(DateTime date) => date.ToString("dd.MM.yyyy");
}
