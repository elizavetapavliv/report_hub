using Duende.IdentityModel;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Exadel.ReportHub.RA.Abstract;
using System.Security.Claims;

namespace Exadel.ReportHub.Identity;

public class ProfileService(IUserRepository userRepository, IUserAssignmentRepository userAssignmentRepository) : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var userId = Guid.Parse(context.Subject.GetSubjectId());
        var user = await userRepository.GetByIdAsync(userId, CancellationToken.None);
        var userRoles = await userAssignmentRepository.GetUserRolesAsync(userId, CancellationToken.None);

        var claims = new List<Claim>
        {
            new Claim(JwtClaimTypes.Name, user.FullName),
            new Claim(JwtClaimTypes.Email, user.Email)
        };

        foreach (var role in userRoles)
        { 
            claims.Add(new Claim(JwtClaimTypes.Role, role.ToString()));
        }

        context.IssuedClaims.AddRange(claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var subject = context.Subject.GetSubjectId();
        context.IsActive = await userRepository.IsActiveAsync(Guid.Parse(subject), CancellationToken.None);
    }
}

