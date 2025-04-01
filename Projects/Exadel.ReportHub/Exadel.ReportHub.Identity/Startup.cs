using Exadel.ReportHub.Identity.Registrations;

namespace Exadel.ReportHub.IDServer;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddIdentity();

        services.AddMongo();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseIdentityServer();
    }
}
