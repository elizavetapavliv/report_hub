using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Exadel.ReportHub.RA.Abstract;

namespace Exadel.ReportHub.IDServer.Stores;

public class IdentityResourceStore(IIdentityRepository identityRepository) : IResourceStore
{
    public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
    {
        return await identityRepository.GetByNamesAsync<ApiResource>(apiResourceNames, CancellationToken.None);
    }

    public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        return await identityRepository.GetApiResourcesByScopeNameAsync(scopeNames, CancellationToken.None);
    }

    public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
    {
        return await identityRepository.GetByNamesAsync<ApiScope>(scopeNames, CancellationToken.None);
    }

    public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        return await identityRepository.GetByNamesAsync<IdentityResource>(scopeNames, CancellationToken.None);
    }

    public async Task<Resources> GetAllResourcesAsync()
    {
        var identityResources = await identityRepository.GetAllAsync<IdentityResource>(CancellationToken.None);
        var apiResources = await identityRepository.GetAllAsync<ApiResource>(CancellationToken.None);
        var apiScopes = await identityRepository.GetAllAsync<ApiScope>(CancellationToken.None);

        return new Resources(identityResources, apiResources, apiScopes);
    }
}
