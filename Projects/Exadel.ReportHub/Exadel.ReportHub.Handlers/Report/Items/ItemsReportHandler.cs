using System.Globalization;
using ErrorOr;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Helpers;
using Exadel.ReportHub.Export.Abstract.Models;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.Report.Items;

public record ItemsReportRequest(Guid ClientId, ExportFormat Format) : IRequest<ErrorOr<ExportResult>>;

public class ItemsReportHandler(IItemRepository itemRepository, IInvoiceRepository invoiceRepository, IExportStrategyFactory exportStrategyFactory)
    : IRequestHandler<ItemsReportRequest, ErrorOr<ExportResult>>
{
    public async Task<ErrorOr<ExportResult>> Handle(ItemsReportRequest request, CancellationToken cancellationToken)
    {
        var exportStrategy = exportStrategyFactory.GetStrategy(request.Format);

        var itemPricesTask = itemRepository.GetClientItemPricesAsync(request.ClientId, cancellationToken);
        var countsTask = invoiceRepository.GetClientItemsCountAsync(request.ClientId, cancellationToken);

        await Task.WhenAll(itemPricesTask, countsTask);
        if (itemPricesTask.Result.Count == 0)
        {
            return new ExportResult
            {
                Stream = Stream.Null,
                FileName = $"ItemsReport_{DateTime.UtcNow.Date.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                           $"{ExportFormatHelper.GetFileExtension(request.Format)}",
                ContentType = ExportFormatHelper.GetContentType(request.Format)
            };
        }

        var report = new ItemsReport
        {
            MostSoldItemId = countsTask.Result.MaxBy(x => x.Value).Key,
            AveragePrice = itemPricesTask.Result.Average(x => x.Value),
            AverageRevenue = itemPricesTask.Result.Select(x => x.Value * countsTask.Result.GetValueOrDefault(x.Key)).Average(),
            ReportDate = DateTime.UtcNow
        };

        var stream = await exportStrategy.ExportAsync(report, cancellationToken);

        return new ExportResult
        {
            Stream = stream,
            FileName = $"ItemsReport_{report.ReportDate.Date.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                $"{ExportFormatHelper.GetFileExtension(request.Format)}",
            ContentType = ExportFormatHelper.GetContentType(request.Format)
        };
    }
}
