﻿using System.Net.Mail;
using AutoFixture;
using Exadel.ReportHub.Data.Enums;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.Email.Abstract;
using Exadel.ReportHub.Email.Models;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Handlers.Managers.Report;
using Exadel.ReportHub.Handlers.Report.Send;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Report;
using Exadel.ReportHub.Tests.Abstracts;
using Microsoft.Extensions.Logging;
using Moq;

namespace Exadel.ReportHub.Tests.Report.Send;

[TestFixture]
public class SendReportsHandlerTests : BaseTestFixture
{
    private Mock<IReportManager> _reportManagerMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IEmailSender> _emailSenderMock;
    private Mock<ILogger<SendReportsHandler>> _loggerMock;
    private SendReportsHandler _handler;

    [SetUp]
    public void Setup()
    {
        _reportManagerMock = new Mock<IReportManager>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _emailSenderMock = new Mock<IEmailSender>();
        _loggerMock = new Mock<ILogger<SendReportsHandler>>();

        _handler = new SendReportsHandler(
            _reportManagerMock.Object,
            _userRepositoryMock.Object,
            _emailSenderMock.Object,
            _loggerMock.Object);
    }

    [Test]
    public async Task SendReports_WithUsers_ProcessesAllUsers()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var currentHour = now.Hour;
        var currentDay = now.Day;
        var currentDayOfWeek = now.DayOfWeek;
        List<Data.Models.User> users = Fixture.Build<Data.Models.User>()
            .With(u => u.NotificationSettings, Fixture.Build<NotificationSettings>()
                .With(ns => ns.ReportPeriod, ReportPeriod.Week)
                .Create())
            .CreateMany(3)
            .ToList();

        _userRepositoryMock.Setup(x => x.GetUsersByNotificationSettingsAsync(
                currentDay,
                currentDayOfWeek,
                currentHour,
                CancellationToken.None))
            .ReturnsAsync(users);

        var result = new ExportResult
        {
            Stream = new MemoryStream(),
            FileName = "report.csv"
        };

        _reportManagerMock.Setup(x => x.GenerateInvoicesReportAsync(
                It.IsAny<ExportReportDTO>(),
                CancellationToken.None))
            .ReturnsAsync(result);

        _reportManagerMock.Setup(x => x.GenerateItemsReportAsync(
                It.IsAny<ExportReportDTO>(),
                CancellationToken.None))
            .ReturnsAsync(result);

        _reportManagerMock.Setup(x => x.GeneratePlansReportAsync(
                It.IsAny<ExportReportDTO>(),
                CancellationToken.None))
            .ReturnsAsync(result);

        // Act
        await _handler.Handle(new SendReportsRequest(), CancellationToken.None);

        // Assert
        _emailSenderMock.Verify(x => x.SendAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<List<Attachment>>(),
            It.IsAny<string>(),
            It.IsAny<EmailReportData>(),
            CancellationToken.None),
            Times.Exactly(users.Count));

        _userRepositoryMock.Verify(x => x.GetUsersByNotificationSettingsAsync(
            currentDay,
            currentDayOfWeek,
            currentHour,
            CancellationToken.None),
            Times.Once);
    }

    [Test]
    public async Task SendReports_ReportGenerationFails_LogsError()
    {
        // Arrange
        var user = Fixture.Build<Data.Models.User>()
            .With(u => u.NotificationSettings, Fixture.Build<NotificationSettings>()
                .With(ns => ns.ReportPeriod, ReportPeriod.Week)
                .Create())
            .Create();

        _userRepositoryMock.Setup(x => x.GetUsersByNotificationSettingsAsync(
             It.IsAny<int>(),
             It.IsAny<DayOfWeek>(),
             It.IsAny<int>(),
             CancellationToken.None))
        .ReturnsAsync(new List<Data.Models.User> { user });

        _reportManagerMock.Setup(x => x.GenerateInvoicesReportAsync(
                It.IsAny<ExportReportDTO>(),
                CancellationToken.None))
            .ThrowsAsync(new Exception());

        // Act
        await _handler.Handle(new SendReportsRequest(), CancellationToken.None);

        // Assert
        _loggerMock.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to generate reports", StringComparison.OrdinalIgnoreCase)),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}
