using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Duende.IdentityModel;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Exadel.ReportHub.Blazor;

public class AuthStateProvider(IJSRuntime js) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await js.InvokeAsync<string>("sessionStorage.getItem", OidcConstants.TokenResponse.AccessToken);

        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var identity = ParseClaimsFromJwt(token);
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyLogin(string token)
    {
        var identity = ParseClaimsFromJwt(token);
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void NotifyLogout()
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }

    private ClaimsIdentity ParseClaimsFromJwt(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);

        var claims = token.Claims.ToList();

        var roleClaims = token.Claims.Where(c => c.Type == JwtClaimTypes.Role);
        foreach (var rc in roleClaims)
        {
            claims.Add(new Claim(JwtClaimTypes.Role, rc.Value));
        }

        return new ClaimsIdentity(claims, JwtClaimTypes.JwtTypes.AccessToken);
    }
}
