using Exadel.ReportHub.Host.Services;
using Exadel.ReportHub.SDK.Abstract;

namespace Exadel.ReportHub.Host.Registrations;

public static class ServiceRegistrations
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ISchedulerService, SchedulerService>();
    }
}
