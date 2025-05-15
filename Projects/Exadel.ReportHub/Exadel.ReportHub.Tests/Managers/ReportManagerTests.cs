using AutoFixture;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Models;
using Exadel.ReportHub.Handlers.Managers.Report;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Report;
using Exadel.ReportHub.SDK.Enums;
using Exadel.ReportHub.Tests.Abstracts;
using Moq;

namespace Exadel.ReportHub.Tests.Managers;

[TestFixture]
public class ReportManagerTests : BaseTestFixture
{
    private Mock<IInvoiceRepository> _invoiceRepositoryMock;
    private Mock<IItemRepository> _itemRepositoryMock;
    private Mock<IPlanRepository> _planRepositoryMock;
    private Mock<IClientRepository> _clientRepositoryMock;
    private Mock<IExportStrategyFactory> _exportStrategyFactoryMock;
    private Mock<IExportStrategy> _exportStrategyMock;

    private ReportManager _reportManager;

    [SetUp]
    public void Setup()
    {
        _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
        _itemRepositoryMock = new Mock<IItemRepository>();
        _planRepositoryMock = new Mock<IPlanRepository>();
        _clientRepositoryMock = new Mock<IClientRepository>();
        _exportStrategyFactoryMock = new Mock<IExportStrategyFactory>();
        _exportStrategyMock = new Mock<IExportStrategy>();

        _reportManager = new ReportManager(
            _invoiceRepositoryMock.Object,
            _itemRepositoryMock.Object,
            _planRepositoryMock.Object,
            _clientRepositoryMock.Object,
            _exportStrategyFactoryMock.Object);
    }

    [Test]
    public async Task GenerateInvoicesReportAsync_ValidRequest_ReturnsExportResult()
    {
        // Arrange
        var exportReportDto = Fixture.Build<ExportReportDTO>()
            .With(x => x.Format, ExportFormat.CSV)
            .Create();

        var report = Fixture.Create<Data.Models.InvoicesReport>();
        var currency = "EUR";
        var stream = new MemoryStream();

        _exportStrategyFactoryMock.Setup(x => x.GetStrategyAsync(exportReportDto.Format, CancellationToken.None))
            .ReturnsAsync(_exportStrategyMock.Object);

        _invoiceRepositoryMock.Setup(x => x.GetReportAsync(
                exportReportDto.ClientId,
                exportReportDto.StartDate,
                exportReportDto.EndDate,
                CancellationToken.None))
            .ReturnsAsync(report);

        _clientRepositoryMock.Setup(x => x.GetCurrencyAsync(exportReportDto.ClientId, CancellationToken.None))
            .ReturnsAsync(currency);

        _exportStrategyMock.Setup(x => x.ExportAsync(report, CancellationToken.None))
            .ReturnsAsync(stream);

        // Act
        var result = await _reportManager.GenerateInvoicesReportAsync(exportReportDto, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Stream, Is.EqualTo(stream));
        Assert.That(result.FileName, Does.StartWith("InvoicesReport_"));
        Assert.That(result.FileName, Does.EndWith(".csv"));
        Assert.That(result.ContentType, Is.EqualTo("text/csv"));

        _exportStrategyFactoryMock.Verify(
            x => x.GetStrategyAsync(exportReportDto.Format, CancellationToken.None),
            Times.Once);
        _invoiceRepositoryMock.Verify(
            x => x.GetReportAsync(
                exportReportDto.ClientId,
                exportReportDto.StartDate,
                exportReportDto.EndDate,
                CancellationToken.None),
            Times.Once);
        _clientRepositoryMock.Verify(
            x => x.GetCurrencyAsync(exportReportDto.ClientId, CancellationToken.None),
            Times.Once);
        _exportStrategyMock.Verify(
            x => x.ExportAsync(report, CancellationToken.None),
            Times.Once);
    }

    [Test]
    public async Task GenerateItemsReportAsync_ValidRequest_ReturnsExportResult()
    {
        // Arrange
        var exportReportDto = Fixture.Build<ExportReportDTO>()
            .With(x => x.Format, ExportFormat.Excel)
            .Create();

        var itemPrices = new Dictionary<Guid, decimal>
        {
            [Guid.NewGuid()] = 10m,
            [Guid.NewGuid()] = 20m
        };
        var itemCounts = new Dictionary<Guid, int>
        {
            [itemPrices.First().Key] = 5,
            [itemPrices.Last().Key] = 10
        };
        var currency = "EUR";
        var stream = new MemoryStream();

        _exportStrategyFactoryMock.Setup(x => x.GetStrategyAsync(exportReportDto.Format, CancellationToken.None))
            .ReturnsAsync(_exportStrategyMock.Object);

        _itemRepositoryMock.Setup(x => x.GetClientItemPricesAsync(exportReportDto.ClientId, CancellationToken.None))
            .ReturnsAsync(itemPrices);

        _invoiceRepositoryMock.Setup(x => x.GetClientItemsCountAsync(
                exportReportDto.ClientId,
                exportReportDto.StartDate,
                exportReportDto.EndDate,
                CancellationToken.None))
            .ReturnsAsync(itemCounts);

        _clientRepositoryMock.Setup(x => x.GetCurrencyAsync(exportReportDto.ClientId, CancellationToken.None))
            .ReturnsAsync(currency);

        _exportStrategyMock.Setup(x => x.ExportAsync(It.IsAny<ItemsReport>(), CancellationToken.None))
            .ReturnsAsync(stream);

        // Act
        var result = await _reportManager.GenerateItemsReportAsync(exportReportDto, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Stream, Is.EqualTo(stream));
        Assert.That(result.FileName, Does.StartWith("ItemsReport_"));
        Assert.That(result.FileName, Does.EndWith(".xlsx"));
        Assert.That(result.ContentType, Is.EqualTo("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));

        _exportStrategyMock.Verify(
            x => x.ExportAsync(
                It.Is<ItemsReport>(r =>
                    r.MostSoldItemId == itemPrices.Last().Key &&
                    r.AveragePrice == 15m &&
                    r.AverageRevenue == 125m &&
                    r.ClientCurrency == currency),
                CancellationToken.None),
            Times.Once);
    }

    [Test]
    public async Task GeneratePlansReportAsync_ValidRequest_ReturnsExportResult()
    {
        // Arrange
        var exportReportDto = Fixture.Build<ExportReportDTO>()
            .With(x => x.Format, ExportFormat.CSV)
            .Create();

        var plans = Fixture.CreateMany<Data.Models.Plan>(3).ToList();
        var counts = plans.ToDictionary(x => x.Id, x => x.Count);
        var prices = plans.ToDictionary(x => x.ItemId, x => 100m);
        var currency = "EUR";
        var stream = new MemoryStream();

        _exportStrategyFactoryMock.Setup(x => x.GetStrategyAsync(exportReportDto.Format, CancellationToken.None))
            .ReturnsAsync(_exportStrategyMock.Object);

        _planRepositoryMock.Setup(x => x.GetByClientIdAsync(
                exportReportDto.ClientId,
                exportReportDto.StartDate,
                exportReportDto.EndDate,
                CancellationToken.None))
            .ReturnsAsync(plans);

        _invoiceRepositoryMock.Setup(x => x.GetPlansActualCountAsync(plans, CancellationToken.None))
            .ReturnsAsync(counts);

        _itemRepositoryMock.Setup(x => x.GetClientItemPricesAsync(exportReportDto.ClientId, CancellationToken.None))
            .ReturnsAsync(prices);

        _clientRepositoryMock.Setup(x => x.GetCurrencyAsync(exportReportDto.ClientId, CancellationToken.None))
            .ReturnsAsync(currency);

        _exportStrategyMock.Setup(x => x.ExportAsync(It.IsAny<IEnumerable<PlanReport>>(), CancellationToken.None))
            .ReturnsAsync(stream);

        // Act
        var result = await _reportManager.GeneratePlansReportAsync(exportReportDto, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Stream, Is.EqualTo(stream));
        Assert.That(result.FileName, Does.StartWith("PlansReport_"));
        Assert.That(result.FileName, Does.EndWith(".csv"));
        Assert.That(result.ContentType, Is.EqualTo("text/csv"));

        _exportStrategyMock.Verify(
            x => x.ExportAsync(
                It.Is<List<PlanReport>>(reports =>
                    reports.Count == 3 &&
                    reports.All(r => r.ClientCurrency == currency) &&
                    reports.All(r => r.Revenue == 100m * r.PlannedCount)),
                CancellationToken.None),
            Times.Once);
    }
}
