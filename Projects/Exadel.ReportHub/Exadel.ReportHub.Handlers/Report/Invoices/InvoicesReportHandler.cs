using System.Globalization;
using ErrorOr;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Helpers;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.Report.Invoices;

public record InvoicesReportRequest(Guid ClientId, ExportFormat Format) : IRequest<ErrorOr<ExportResult>>;

public class InvoicesReportHandler(IInvoiceRepository invoiceRepository, IExportStrategyFactory exportStrategyFactory)
    : IRequestHandler<InvoicesReportRequest, ErrorOr<ExportResult>>
{
    public async Task<ErrorOr<ExportResult>> Handle(InvoicesReportRequest request, CancellationToken cancellationToken)
    {
        var exportStrategyTask = exportStrategyFactory.GetStrategyAsync(request.Format, cancellationToken);
        var reportTask = invoiceRepository.GetReportAsync(request.ClientId, cancellationToken);

        await Task.WhenAll(exportStrategyTask, reportTask);

        if (reportTask.Result is not null)
        {
            reportTask.Result.ReportDate = DateTime.UtcNow;
        }

        var stream = await exportStrategyTask.Result.ExportAsync(reportTask.Result ?? new Data.Models.InvoicesReport { ReportDate = DateTime.UtcNow }, cancellationToken);

        return new ExportResult
        {
            Stream = stream,
            FileName = $"InvoicesReport_{DateTime.UtcNow.Date.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                $"{ExportFormatHelper.GetFileExtension(request.Format)}",
            ContentType = ExportFormatHelper.GetContentType(request.Format)
        };
    }
}
