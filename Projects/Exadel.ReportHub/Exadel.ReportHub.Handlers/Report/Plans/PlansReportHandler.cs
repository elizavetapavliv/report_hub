using System.Globalization;
using ErrorOr;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Helpers;
using Exadel.ReportHub.Export.Abstract.Models;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.Report.Plans;

public record PlansReportRequest(Guid ClientId, ExportFormat Format) : IRequest<ErrorOr<ExportResult>>;

public class PlansReportHandler(IPlanRepository planRepository, IInvoiceRepository invoiceRepository, IExportStrategyFactory exportStrategyFactory)
    : IRequestHandler<PlansReportRequest, ErrorOr<ExportResult>>
{
    public async Task<ErrorOr<ExportResult>> Handle(PlansReportRequest request, CancellationToken cancellationToken)
    {
        var plans = await planRepository.GetByClientIdAsync(request.ClientId, cancellationToken);

        var exportStrategyTask = exportStrategyFactory.GetStrategyAsync(request.Format, cancellationToken);
        var countsTask = invoiceRepository.GetPlansActualCountAsync(plans, cancellationToken);

        await Task.WhenAll(exportStrategyTask, countsTask);

        var reports = plans.Select(x => new PlansReport
        {
            TargetItemId = x.ItemId,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            PlannedCount = x.Count,
            ActualCount = countsTask.Result.GetValueOrDefault(x.Id),
            ReportDate = DateTime.UtcNow
        }).ToList();

        var stream = await exportStrategyTask.Result.ExportAsync(reports, cancellationToken);

        return new ExportResult
        {
            Stream = stream,
            FileName = $"PlansReport_{DateTime.UtcNow.Date.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                       $"{ExportFormatHelper.GetFileExtension(request.Format)}",
            ContentType = ExportFormatHelper.GetContentType(request.Format)
        };
    }
}
