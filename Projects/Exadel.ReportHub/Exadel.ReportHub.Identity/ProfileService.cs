using Duende.IdentityModel;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Exadel.ReportHub.RA.Abstract;
using System.Security.Claims;

namespace Exadel.ReportHub.Identity;

public class ProfileService(IUserRepository userRepository) : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var subject = context.Subject.GetSubjectId();
        var user = await userRepository.GetByIdAsync(Guid.Parse(subject), CancellationToken.None);

        var claims = new List<Claim>
        {
            new Claim(JwtClaimTypes.Role, user.Role.ToString())
        };

        context.IssuedClaims.AddRange(claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var subject = context.Subject.GetSubjectId();
        var user = await userRepository.GetByIdAsync(Guid.Parse(subject), CancellationToken.None);
        context.IsActive = user != null && user.IsActive;
    }
}

