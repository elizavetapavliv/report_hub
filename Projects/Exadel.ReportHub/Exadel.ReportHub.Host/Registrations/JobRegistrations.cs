using Exadel.ReportHub.Host.Job;
using Exadel.ReportHub.SDK.Abstract;

namespace Exadel.ReportHub.Host.Registrations;

public static class JobRegistrations
{
    public static void AddJobs(this IServiceCollection services)
    {
        var assembly = typeof(PingJob).Assembly;

        var types = assembly.GetTypes().Where(type => typeof(IJob).IsAssignableFrom(type));

        foreach (var type in types)
        {
            services.AddSingleton(typeof(IJob), type);
        }
    }
}
