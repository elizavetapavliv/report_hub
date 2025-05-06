namespace Exadel.ReportHub.Export.Abstract;

public interface IExportStrategyFactory
{
    Task<IExportStrategy> GetStrategyAsync(ExportFormat format, CancellationToken cancellationToken);
}
