using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Host.Services;
using Exadel.ReportHub.SDK.Abstract;

namespace Exadel.ReportHub.Host.Registrations;

[ExcludeFromCodeCoverage]
public static class ServiceRegistrations
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IInvoiceService, InvoicesService>();
        services.AddScoped<IReportService, ReportsService>();

        return services;
    }
}
