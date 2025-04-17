using Exadel.ReportHub.Host.Services;
using Hangfire;
using Hangfire.MemoryStorage;

namespace Exadel.ReportHub.Host.Registrations;

public static class HangfireRegistrations
{
    public static void AddHangfireMemoryStorage(this IServiceCollection services)
    {
        services.AddHangfire(config =>
            config
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMemoryStorage());

        services.AddHangfireServer();
        services.AddSingleton<SchedulerService>();
    }

    public static void UseHangfireDashboardAndJobs(this IApplicationBuilder app)
    {
        app.UseHangfireDashboard();

        var scheduler = app.ApplicationServices.GetRequiredService<SchedulerService>();
        scheduler.StartJobs();
    }
}
