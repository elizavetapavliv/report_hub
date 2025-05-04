namespace Exadel.ReportHub.Export.Abstract;

public interface IExportStrategyFactory
{
    IExportStrategy GetStrategy(ExportFormat format);
}
