using System.Globalization;
using ErrorOr;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Helpers;
using Exadel.ReportHub.Handlers.Managers;
using Exadel.ReportHub.SDK.DTOs.Report;
using MediatR;

namespace Exadel.ReportHub.Handlers.Report.Items;

public record ItemsReportRequest(ExportReportDTO ExportReportDto) : IRequest<ErrorOr<ExportResult>>;

public class ItemsReportHandler(IReportManager reportManager) : IRequestHandler<ItemsReportRequest, ErrorOr<ExportResult>>
{
    public async Task<ErrorOr<ExportResult>> Handle(ItemsReportRequest request, CancellationToken cancellationToken)
    {
        var stream = await reportManager.GenerateItemsReportAsync(request.ExportReportDto, cancellationToken);

        return new ExportResult
        {
            Stream = stream,
            FileName = $"ItemsReport_{DateTime.Today.ToString(Export.Abstract.Constants.Format.Date, CultureInfo.InvariantCulture)}" +
                $"{ExportFormatHelper.GetFileExtension(request.ExportReportDto.Format)}",
            ContentType = ExportFormatHelper.GetContentType(request.ExportReportDto.Format)
        };
    }
}
