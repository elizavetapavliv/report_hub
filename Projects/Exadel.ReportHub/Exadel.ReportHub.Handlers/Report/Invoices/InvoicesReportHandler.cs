using System.Globalization;
using ErrorOr;
using Exadel.ReportHub.Data.Enums;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Helpers;
using Exadel.ReportHub.Export.Abstract.Models;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.Report.Invoices;

public record InvoicesReportRequest(Guid ClientId, ExportFormat Format) : IRequest<ErrorOr<ExportResult>>;

public class InvoicesReportHandler(IInvoiceRepository invoiceRepository, IExportStrategyFactory exportStrategyFactory)
    : IRequestHandler<InvoicesReportRequest, ErrorOr<ExportResult>>
{
    public async Task<ErrorOr<ExportResult>> Handle(InvoicesReportRequest request, CancellationToken cancellationToken)
    {
        var exportStrategy = exportStrategyFactory.GetStrategy(request.Format);

        var monthInvoices = await invoiceRepository.GetGroupedByMonthAsync(request.ClientId, cancellationToken);

        if (monthInvoices.Count == 0)
        {
            return new ExportResult
            {
                Stream = Stream.Null,
                FileName = $"InvoicesReport_{DateTime.UtcNow.Date.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                           $"{ExportFormatHelper.GetFileExtension(request.Format)}",
                ContentType = ExportFormatHelper.GetContentType(request.Format)
            };
        }

        var totalCount = monthInvoices.Sum(x => x.Value.Count);
        var totalAmount = monthInvoices.Sum(x => x.Value.Sum(x => x.ClientCurrencyAmount));

        var report = new InvoicesReport
        {
            ReportDate = DateTime.UtcNow,
            TotalCount = totalCount,
            AverageMonthCount = (int)Math.Round(monthInvoices.Average(x => x.Value.Count)),
            TotalAmount = totalAmount,
            AverageAmount = totalAmount / totalCount,
            UnpaidCount = GetStatusCount(monthInvoices, PaymentStatus.Unpaid),
            OverdueCount = GetStatusCount(monthInvoices, PaymentStatus.Overdue),
            PaidOnTimeCount = GetStatusCount(monthInvoices, PaymentStatus.PaidOnTime),
            PaidLateCount = GetStatusCount(monthInvoices, PaymentStatus.PaidLate)
        };

        var stream = await exportStrategy.ExportAsync(report, cancellationToken);

        return new ExportResult
        {
            Stream = stream,
            FileName = $"InvoicesReport_{report.ReportDate.Date.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                $"{ExportFormatHelper.GetFileExtension(request.Format)}",
            ContentType = ExportFormatHelper.GetContentType(request.Format)
        };
    }

    private int GetStatusCount(Dictionary<(int, int), List<Data.Models.Invoice>> monthInvoices, PaymentStatus status)
    {
        return monthInvoices.Sum(x => x.Value.Count(x => x.PaymentStatus == status));
    }
}
