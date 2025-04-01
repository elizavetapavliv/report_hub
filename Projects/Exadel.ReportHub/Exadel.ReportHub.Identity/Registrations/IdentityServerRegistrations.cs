using Exadel.ReportHub.IDServer.Stores;

namespace Exadel.ReportHub.Identity.Registrations;

public static class IdentityServerRegistrations
{
    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentityServer()
            .AddClientStore<IdentityClientStore>()
            .AddResourceStore<IdentityResourceStore>()
            .AddDeveloperSigningCredential();
    }
}
