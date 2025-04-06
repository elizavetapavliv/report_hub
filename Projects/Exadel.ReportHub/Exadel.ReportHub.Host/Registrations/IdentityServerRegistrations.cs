using Duende.IdentityServer.Services;
using Exadel.ReportHub.Identity;
using Exadel.ReportHub.Identity.Stores;

namespace Exadel.ReportHub.Host.Registrations;

public static class IdentityServerRegistrations
{
    public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityServer(options =>
        {
            options.IssuerUri = configuration["Authority"];
        })
            .AddClientStore<IdentityClientStore>()
            .AddResourceStore<IdentityResourceStore>()
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
            .AddProfileService<ProfileService>()
            .AddDeveloperSigningCredential(false);

        services.AddScoped<IProfileService, ProfileService>();
    }
}
