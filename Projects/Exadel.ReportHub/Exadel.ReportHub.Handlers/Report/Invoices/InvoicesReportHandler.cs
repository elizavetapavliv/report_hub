using System.Globalization;
using ErrorOr;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Helpers;
using Exadel.ReportHub.Handlers.Managers;
using Exadel.ReportHub.SDK.DTOs.Report;
using MediatR;

namespace Exadel.ReportHub.Handlers.Report.Invoices;

public record InvoicesReportRequest(ExportReportDTO ExportReportDto) : IRequest<ErrorOr<ExportResult>>;

public class InvoicesReportHandler(IReportManager reportManager) : IRequestHandler<InvoicesReportRequest, ErrorOr<ExportResult>>
{
    public async Task<ErrorOr<ExportResult>> Handle(InvoicesReportRequest request, CancellationToken cancellationToken)
    {
        var stream = await reportManager.GenerateInvoicesReportAsync(request.ExportReportDto, cancellationToken);

        return new ExportResult
        {
            Stream = stream,
            FileName = $"InvoicesReport_{DateTime.Today.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                $"{ExportFormatHelper.GetFileExtension(request.ExportReportDto.Format)}",
            ContentType = ExportFormatHelper.GetContentType(request.ExportReportDto.Format)
        };
    }
}
