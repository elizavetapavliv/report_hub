namespace Exadel.ReportHub.Export.Abstract;

public class ExportStrategyFactory(IEnumerable<IExportStrategy> strategies) : IExportStrategyFactory
{
    public IExportStrategy GetStrategy(ExportFormat format)
    {
        var strategy = strategies.FirstOrDefault(x => x.Satisfy(format));
        return strategy ?? throw new ArgumentException($"Unsupported export format: {format}");
    }
}
