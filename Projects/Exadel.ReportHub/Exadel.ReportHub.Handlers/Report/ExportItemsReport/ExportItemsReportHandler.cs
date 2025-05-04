using System.Globalization;
using ErrorOr;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Helpers;
using Exadel.ReportHub.Export.Abstract.Models;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.Report.ExportItemsReport;

public record ExportItemsReportRequest(Guid ClientId, ExportFormat Format) : IRequest<ErrorOr<ExportResult>>;

public class ExportItemsReportHandler(IItemRepository itemRepository, IInvoiceRepository invoiceRepository, IExportStrategyFactory exportStrategyFactory)
    : IRequestHandler<ExportItemsReportRequest, ErrorOr<ExportResult>>
{
    public async Task<ErrorOr<ExportResult>> Handle(ExportItemsReportRequest request, CancellationToken cancellationToken)
    {
        var exportStrategy = exportStrategyFactory.GetStrategy(request.Format);

        var itemsTask = itemRepository.GetByClientIdAsync(request.ClientId, cancellationToken);
        var countsTask = invoiceRepository.GetItemsCountByClientIdAsync(request.ClientId, cancellationToken);

        await Task.WhenAll(itemsTask, countsTask);
        if (itemsTask.Result.Count == 0)
        {
            return Error.NotFound();
        }

        var exportModel = new ItemsReportModel
        {
            MostSoldItemId = countsTask.Result.MaxBy(x => x.Value).Key,
            AveragePrice = itemsTask.Result.Average(x => x.Price),
            AverageRevenue = itemsTask.Result.Select(x => x.Price * countsTask.Result.GetValueOrDefault(x.Id)).Average(),
            ReportDate = DateTime.UtcNow
        };

        var stream = await exportStrategy.GenerateAsync(exportModel, cancellationToken);

        return new ExportResult
        {
            Stream = stream,
            FileName = $"ItemsReport_{exportModel.ReportDate.Date.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                $"{ExportFormatHelper.GetFileExtension(request.Format)}",
            ContentType = ExportFormatHelper.GetContentType(request.Format)
        };
    }
}
