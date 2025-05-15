using System.Globalization;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.Ecb.Abstract;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Helpers;
using Exadel.ReportHub.Export.Abstract.Models;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Report;

namespace Exadel.ReportHub.Handlers.Managers.Report;

public class ReportManager(IInvoiceRepository invoiceRepository, IItemRepository itemRepository, IPlanRepository planRepository,
    IClientRepository clientRepository, ICurrencyConverter currencyConverter, IExportStrategyFactory exportStrategyFactory) : IReportManager
{
    public async Task<ExportResult> GenerateInvoicesReportAsync(ExportReportDTO exportReportDto, CancellationToken cancellationToken)
    {
        var exportStrategyTask = exportStrategyFactory.GetStrategyAsync(exportReportDto.Format, cancellationToken);
        var reportTask = invoiceRepository.GetReportAsync(exportReportDto.ClientId,
            exportReportDto.StartDate, exportReportDto.EndDate, cancellationToken);
        var clientCurrencyTask = clientRepository.GetCurrencyAsync(exportReportDto.ClientId, cancellationToken);

        await Task.WhenAll(exportStrategyTask, reportTask, clientCurrencyTask);

        var report = reportTask.Result ?? new InvoicesReport();
        report.ClientCurrency = clientCurrencyTask.Result;
        report.ReportDate = DateTime.UtcNow;

        var stream = await exportStrategyTask.Result.ExportAsync(report, cancellationToken: cancellationToken);

        return new ExportResult
        {
            Stream = stream,
            FileName = $"InvoicesReport_{DateTime.Today.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                       $"{ExportFormatHelper.GetFileExtension(exportReportDto.Format)}",
            ContentType = ExportFormatHelper.GetContentType(exportReportDto.Format)
        };
    }

    public async Task<ExportResult> GenerateItemsReportAsync(ExportReportDTO exportReportDto, CancellationToken cancellationToken)
    {
        var exportStrategyTask = exportStrategyFactory.GetStrategyAsync(exportReportDto.Format, cancellationToken);

        var itemPricesTask = itemRepository.GetClientItemNamesPricesAsync(exportReportDto.ClientId, cancellationToken);
        var countsTask = invoiceRepository.GetClientItemsCountAsync(exportReportDto.ClientId,
            exportReportDto.StartDate, exportReportDto.EndDate, cancellationToken);
        var currencyTask = clientRepository.GetCurrencyAsync(exportReportDto.ClientId, cancellationToken);

        await Task.WhenAll(exportStrategyTask, itemPricesTask, countsTask, currencyTask);
        var clientCurrency = currencyTask.Result;
        var conversionTasks = itemPricesTask.Result.Select(async x =>
        {
            x.Value.Price = await currencyConverter.ConvertAsync(x.Value.Price, x.Value.Currency, clientCurrency, DateTime.UtcNow, cancellationToken);
            x.Value.Currency = clientCurrency;
        });
        await Task.WhenAll(conversionTasks);
        var report = new ItemsReport
        {
            MostSoldItemName = countsTask.Result.Count > 0 ?
                itemPricesTask.Result[countsTask.Result.MaxBy(x => x.Value).Key].Name :
                "-",
            AveragePrice = itemPricesTask.Result.Count > 0 ?
                itemPricesTask.Result.Average(x => x.Value.Price) :
                0,
            AverageRevenue = countsTask.Result.Count > 0 && itemPricesTask.Result.Count > 0 ?
                itemPricesTask.Result.Select(x => x.Value.Price * countsTask.Result.GetValueOrDefault(x.Key)).Average() :
                0,
            ClientCurrency = currencyTask.Result,
            ReportDate = DateTime.UtcNow
        };

        var stream = await exportStrategyTask.Result.ExportAsync(report, cancellationToken: cancellationToken);

        return new ExportResult
        {
            Stream = stream,
            FileName = $"ItemsReport_{DateTime.Today.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                       $"{ExportFormatHelper.GetFileExtension(exportReportDto.Format)}",
            ContentType = ExportFormatHelper.GetContentType(exportReportDto.Format)
        };
    }

    public async Task<ExportResult> GeneratePlansReportAsync(ExportReportDTO exportReportDto, CancellationToken cancellationToken)
    {
        var exportStrategyTask = exportStrategyFactory.GetStrategyAsync(exportReportDto.Format, cancellationToken);
        var plansTask = planRepository.GetByClientIdAsync(exportReportDto.ClientId,
            exportReportDto.StartDate, exportReportDto.EndDate, cancellationToken);
        var reports = new List<PlanReport>();

        await Task.WhenAll(exportStrategyTask, plansTask);
        var plans = plansTask.Result;
        var chartData = new ChartData();
        if (plans.Any())
        {
            var countsTask = invoiceRepository.GetPlansActualCountAsync(plans, cancellationToken);
            var itemsTask = itemRepository.GetClientItemNamesPricesAsync(exportReportDto.ClientId, cancellationToken);
            var clientCurrencyTask = clientRepository.GetCurrencyAsync(exportReportDto.ClientId, cancellationToken);

            await Task.WhenAll(countsTask, itemsTask, clientCurrencyTask);
            var itemInfos = itemsTask.Result;
            var counts = countsTask.Result;
            var clientCurrency = clientCurrencyTask.Result;

            reports = plans.Select(x =>
            {
                var item = itemInfos[x.ItemId];

                return new PlanReport
                {
                    TargetItemName = item.Name,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    PlannedCount = x.Count,
                    ActualCount = counts[x.Id],
                    Revenue = item.Price * counts[x.Id],
                    ItemCurrency = item.Currency,
                    ReportDate = DateTime.UtcNow
                };
            }).OrderByDescending(x => x.StartDate)
                .ToList();

            var conversionTasks = reports.GroupBy(x =>
                new
                {
                    Name = x.TargetItemName,
                    Currency = x.ItemCurrency
                },
                x => x.Revenue)
                .Select(async group =>
                new
                {
                    Name = group.Key.Name,
                    Revenue = await currencyConverter.ConvertAsync(group.Sum(), group.Key.Currency, clientCurrency, DateTime.UtcNow, cancellationToken)
                });
            var conversionResults = await Task.WhenAll(conversionTasks);

            chartData = new ChartData
            {
                ChartTitle = string.Format(Export.Abstract.Constants.ChartInfo.PlansChartTitle, clientCurrency),
                NamesTitle = Export.Abstract.Constants.ChartInfo.PlansNamesTitle,
                ValuesTitle = Export.Abstract.Constants.ChartInfo.PlansValuesTitle,
                NameValues = conversionResults.ToDictionary(k => k.Name, v => v.Revenue)
            };
        }

        var stream = await exportStrategyTask.Result.ExportAsync(reports, chartData, cancellationToken);

        return new ExportResult
        {
            Stream = stream,
            FileName = $"PlansReport_{DateTime.Today.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                       $"{ExportFormatHelper.GetFileExtension(exportReportDto.Format)}",
            ContentType = ExportFormatHelper.GetContentType(exportReportDto.Format)
        };
    }
}
