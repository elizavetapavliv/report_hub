namespace Exadel.ReportHub.Export.Abstract;

public interface IExportStrategyFactory
{
    IExportStrategy Create(ExportFormat format);
}
