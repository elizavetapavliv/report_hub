using Duende.IdentityModel;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Exadel.ReportHub.Data;
using Exadel.ReportHub.Data.Enums;
using Exadel.ReportHub.RA.Abstract;
using System.Security.Claims;

namespace Exadel.ReportHub.Identity;

public class ProfileService(IUserRepository userRepository, IUserAssignmentRepository userAssignmentRepository) : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var userId = Guid.Parse(context.Subject.GetSubjectId());
        var user = await userRepository.GetByIdAsync(userId, CancellationToken.None);
        var userGlobalRole = await userAssignmentRepository.GetRoleAsync(userId, Constants.GlobalClientId, CancellationToken.None);

        var claims = new List<Claim>
        {
            new Claim(JwtClaimTypes.Role, userGlobalRole.ToString()),
            new Claim(JwtClaimTypes.Name, user.FullName),
            new Claim(JwtClaimTypes.Email, user.Email)
        };

        if (userGlobalRole == UserRole.SuperAdmin)
        {
            claims.Add(new Claim(JwtClaimTypes.Role, UserRole.Regular.ToString()));
        }

        context.IssuedClaims.AddRange(claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var subject = context.Subject.GetSubjectId();
        context.IsActive = await userRepository.IsActiveAsync(Guid.Parse(subject), CancellationToken.None);
    }
}

