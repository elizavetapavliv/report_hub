using Exadel.ReportHub.Identity.Registrations;
using Exadel.ReportHub.IDServer.Stores;

namespace Exadel.ReportHub.IDServer;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddIdentityServer()
            .AddClientStore<IdentityClientStore>()
            .AddResourceStore<IdentityResourceStore>()
            .AddDeveloperSigningCredential();

        services.AddMongo();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseIdentityServer();
    }
}
