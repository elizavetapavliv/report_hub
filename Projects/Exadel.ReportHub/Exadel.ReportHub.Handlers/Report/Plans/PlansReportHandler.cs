using System.Globalization;
using ErrorOr;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Helpers;
using Exadel.ReportHub.Handlers.Managers;
using Exadel.ReportHub.SDK.DTOs.Report;
using MediatR;

namespace Exadel.ReportHub.Handlers.Report.Plans;

public record PlansReportRequest(ExportReportDTO ExportReportDto) : IRequest<ErrorOr<ExportResult>>;

public class PlansReportHandler(IReportManager reportManager) : IRequestHandler<PlansReportRequest, ErrorOr<ExportResult>>
{
    public async Task<ErrorOr<ExportResult>> Handle(PlansReportRequest request, CancellationToken cancellationToken)
    {
        var stream = await reportManager.GeneratePlansReportAsync(request.ExportReportDto, cancellationToken);

        return new ExportResult
        {
            Stream = stream,
            FileName = $"PlansReport_{DateTime.Today.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                $"{ExportFormatHelper.GetFileExtension(request.ExportReportDto.Format)}",
            ContentType = ExportFormatHelper.GetContentType(request.ExportReportDto.Format)
        };
    }
}
