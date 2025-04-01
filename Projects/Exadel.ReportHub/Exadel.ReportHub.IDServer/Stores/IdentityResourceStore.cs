using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;

namespace Exadel.ReportHub.IDServer.Stores;

public class IdentityResourceStore : IResourceStore
{
    public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
    {
        return Task.FromResult<IEnumerable<ApiResource>>(new ApiResource[]
        {
            new ApiResource("myApi")
            {
                Scopes = new List<string>{ "myApi.read","myApi.write" },
                ApiSecrets = new List<Secret>{ new Secret("supersecret".Sha256()) }
            }
        });
    }

    public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        return Task.FromResult<IEnumerable<ApiResource>>(new ApiResource[]
        {
            new ApiResource("myApi")
            {
                Scopes = new List<string>{ "myApi.read","myApi.write" },
                ApiSecrets = new List<Secret>{ new Secret("supersecret".Sha256()) }
            }
        });
    }

    public Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
    {
        return Task.FromResult<IEnumerable<ApiScope>>(new ApiScope[]
        {
            new ApiScope("myApi.read"),
            new ApiScope("myApi.write"),
        });
    }

    public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        return Task.FromResult<IEnumerable<IdentityResource>>(new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        });
    }

    public async Task<Resources> GetAllResourcesAsync()
    {
        var apiResources = await FindApiResourcesByNameAsync(new[] { "string" });
        var identityResources = await FindIdentityResourcesByScopeNameAsync(new[] { "string" });
        var apiScopes = await FindApiScopesByNameAsync(new[] { "string" });

        return new Resources(identityResources, apiResources, apiScopes);
    }
}
