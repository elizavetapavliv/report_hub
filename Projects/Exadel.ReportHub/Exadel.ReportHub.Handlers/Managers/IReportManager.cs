using Exadel.ReportHub.SDK.DTOs.Report;

namespace Exadel.ReportHub.Handlers.Managers;

public interface IReportManager
{
    Task<Stream> GenerateInvoicesReportAsync(ExportReportDTO exportReportDto, CancellationToken cancellationToken);

    Task<Stream> GenerateItemsReportAsync(ExportReportDTO exportReportDto, CancellationToken cancellationToken);

    Task<Stream> GeneratePlansReportAsync(ExportReportDTO exportReportDto, CancellationToken cancellationToken);
}
