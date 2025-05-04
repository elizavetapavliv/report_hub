using Exadel.ReportHub.Csv;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Host.Infrastructure.Export;

namespace Exadel.ReportHub.Host.Registrations;

public static class ExportRegistrations
{
    public static IServiceCollection AddExport(this IServiceCollection services)
    {
        services.AddSingleton<CsvProcessor>();
        services.AddSingleton<IExportStrategyFactory, ExportStrategyFactory>();

        return services;
    }
}
