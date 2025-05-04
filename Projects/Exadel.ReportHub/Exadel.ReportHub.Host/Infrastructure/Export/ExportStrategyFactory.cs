using Exadel.ReportHub.Csv;
using Exadel.ReportHub.Export.Abstract;

namespace Exadel.ReportHub.Host.Infrastructure.Export;

public class ExportStrategyFactory(IServiceProvider provider) : IExportStrategyFactory
{
    public IExportStrategy Create(ExportFormat format)
    {
        return format switch
        {
            ExportFormat.Csv => provider.GetRequiredService<CsvProcessor>()
        };
    }
}
