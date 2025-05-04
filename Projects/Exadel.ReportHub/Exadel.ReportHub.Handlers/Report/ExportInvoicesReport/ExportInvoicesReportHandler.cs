using System.Globalization;
using ErrorOr;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Helpers;
using Exadel.ReportHub.Export.Abstract.Models;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.Report.ExportInvoicesReport;

public record ExportInvoicesReportRequest(Guid ClientId, ExportFormat Format) : IRequest<ErrorOr<ExportResult>>;

public class ExportInvoicesReportHandler(IInvoiceRepository invoiceRepository, IExportStrategyFactory exportStrategyFactory)
    : IRequestHandler<ExportInvoicesReportRequest, ErrorOr<ExportResult>>
{
    public async Task<ErrorOr<ExportResult>> Handle(ExportInvoicesReportRequest request, CancellationToken cancellationToken)
    {
        var exportStrategy = exportStrategyFactory.GetStrategy(request.Format);

        var invoices = await invoiceRepository.GetByClientIdAsync(request.ClientId, cancellationToken);

        if (invoices.Count == 0)
        {
            return Error.NotFound();
        }

        var exportModel = new InvoicesReportModel
        {
            ReportDate = DateTime.UtcNow,
            TotalCount = invoices.Count,
            AverageMonthCount = (int)Math.Round(invoices.GroupBy(x => new { x.IssueDate.Year, x.IssueDate.Month }).Average(g => g.Count())),
            TotalAmount = invoices.Sum(x => x.ClientCurrencyAmount),
            AverageAmount = invoices.Average(x => x.ClientCurrencyAmount),
            UnpaidCount = invoices.Count(x => x.PaymentStatus == Data.Enums.PaymentStatus.Unpaid),
            OverdueCount = invoices.Count(x => x.PaymentStatus == Data.Enums.PaymentStatus.Overdue),
            PaidOnTimeCount = invoices.Count(x => x.PaymentStatus == Data.Enums.PaymentStatus.PaidOnTime),
            PaidLateCount = invoices.Count(x => x.PaymentStatus == Data.Enums.PaymentStatus.PaidLate)
        };

        var stream = await exportStrategy.GenerateAsync(exportModel, cancellationToken);

        return new ExportResult
        {
            Stream = stream,
            FileName = $"InvoicesReport_{exportModel.ReportDate.Date.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                $"{ExportFormatHelper.GetFileExtension(request.Format)}",
            ContentType = ExportFormatHelper.GetContentType(request.Format)
        };
    }
}
