using Duende.IdentityServer.Validation;
using ErrorOr;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Helpers;
using Exadel.ReportHub.Export.Abstract.Models;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Export;
using MediatR;

namespace Exadel.ReportHub.Handlers.Report.ExportInvoicesReport;

public record ExportInvoicesReportRequest(ExportDTO ExportDto) : IRequest<ErrorOr<ExportResult>>;

public class ExportInvoicesReportHandler(IInvoiceRepository invoiceRepository, IExportStrategyFactory exportStrategyFactory) : IRequestHandler<ExportInvoicesReportRequest, ErrorOr<ExportResult>>
{
    public async Task<ErrorOr<ExportResult>> Handle(ExportInvoicesReportRequest request, CancellationToken cancellationToken)
    {
        var exportStrategy = exportStrategyFactory.Create((ExportFormat)request.ExportDto.Format);

        var invoices = await invoiceRepository.GetByClientIdAsync(request.ExportDto.ClientId, cancellationToken);

        if (invoices.Count == 0)
        {
            return Error.NotFound();
        }

        var grouping = invoices.GroupBy(x => new { x.IssueDate.Year, x.IssueDate.Month }).ToList();
        var exportModel = new InvoiceReportModel
        {
            TotalCount = invoices.Count,
            AverageMonthCount = (int)Math.Round(grouping.Select(g => g.Count()).Average()),
            TotalAmount = invoices.Sum(x => x.Amount),
            AverageAmount = grouping.Select(g => g.Sum(x => x.Amount)).Average(),
            UnpaidCount = invoices.Count(x => x.PaymentStatus == Data.Enums.PaymentStatus.Unpaid),
            PaidCount = invoices.Count(x => x.PaymentStatus == Data.Enums.PaymentStatus.Paid),
            ReportDate = DateTime.UtcNow
        };

        var stream = await exportStrategy.GenerateAsync(exportModel, cancellationToken);

        return new ExportResult
        {
            Stream = stream,
            FileName = $"Invoices{ExportFormatHelper.GetFileExtension((ExportFormat)request.ExportDto.Format)}",
            ContentType = ExportFormatHelper.GetContentType((ExportFormat)request.ExportDto.Format)
        };
    }
}
