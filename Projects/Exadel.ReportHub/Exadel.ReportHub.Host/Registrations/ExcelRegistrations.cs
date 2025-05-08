using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Excel;
using Exadel.ReportHub.Excel.Abstract;

namespace Exadel.ReportHub.Host.Registrations;

[ExcludeFromCodeCoverage]
public static class ExcelRegistrations
{
    public static IServiceCollection AddExcel(this IServiceCollection services)
    {
        services.AddSingleton<IExcelProcessor, ExcelProcessor>();

        return services;
    }
}
