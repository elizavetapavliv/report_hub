using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;

namespace Exadel.ReportHub.IDServer.Stores;

public class IdentityClientStore : IClientStore
{
    public Task<Client?> FindClientByIdAsync(string clientId)
    {
        return Task.FromResult<Client?>(new Client
        {
            ClientId = "cwm.client",
            ClientName = "Client Credentials Client",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret("secret".Sha256()) },
            AllowedScopes = { "myApi.read" }
        });
    }
}
