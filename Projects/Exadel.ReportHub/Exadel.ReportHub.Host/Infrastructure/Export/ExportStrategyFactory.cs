using Exadel.ReportHub.Csv;
using Exadel.ReportHub.Excel;
using Exadel.ReportHub.Export.Abstract;

namespace Exadel.ReportHub.Host.Infrastructure.Export;

public class ExportStrategyFactory(IServiceProvider provider) : IExportStrategyFactory
{
    public IExportStrategy GetStrategy(ExportFormat format)
    {
        return format switch
        {
            ExportFormat.Csv => provider.GetRequiredService<CsvProcessor>(),
            ExportFormat.Excel => provider.GetRequiredService<ExcelProcessor>(),
            _ => throw new ArgumentException($"Unsupported export format: {format}")
        };
    }
}
