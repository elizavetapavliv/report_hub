using Microsoft.Extensions.DependencyInjection;

namespace Exadel.ReportHub.Handlers;

public static class DependencyIjection
{
    public static void AddHandlers(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyIjection).Assembly));
    }
}
