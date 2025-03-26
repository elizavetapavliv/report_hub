using Exadel.ReportHub.RA;

namespace Exadel.ReportHub.Host.Registration;

public static class MongoRegistration
{
    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        services.AddSingleton<MongoDbContext>();
        return services;
    }
}
