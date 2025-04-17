using Exadel.ReportHub.ExchangeRate;
using Exadel.ReportHub.Host.Services;

namespace Exadel.ReportHub.Host.Registrations;

public static class ExchangeRateRegistrations
{
    public static void AddExchangeRate(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ExchangeRateConfig>(configuration.GetSection("ExchangeRate"));

        services.AddHttpClient<IExchangeRateProvider, ExchangeRateProvider>(client =>
                {
                    client.BaseAddress = new Uri("https://www.ecb.europa.eu/");
                    client.Timeout = TimeSpan.FromSeconds(10);
                });

        services.AddHttpClient<PingService>(client =>
        {
            client.BaseAddress = new Uri("https://report-hub-f3k3.onrender.com");
            client.Timeout = TimeSpan.FromSeconds(10);
        });

        services.AddSingleton<ExchangeRateService>();
    }
}
