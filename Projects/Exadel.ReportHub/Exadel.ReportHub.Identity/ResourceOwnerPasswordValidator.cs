using Duende.IdentityModel;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Exadel.ReportHub.Common;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;
using System.Security.Claims;

namespace Exadel.ReportHub.Identity;

public class ResourceOwnerPasswordValidator(IUserRepository userRepository) : IResourceOwnerPasswordValidator
{
    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var user = await userRepository.GetByEmailAsync(context.UserName, CancellationToken.None);
        if (user == null)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "User does not exist.");
            return;
        }

        var passwordHash = PasswordHasher.GetPasswordHash(context.Password, user.PasswordSalt);
        if (!user.PasswordHash.Equals(passwordHash))
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Incorrect password.");
            return;
        }

        context.Result = new GrantValidationResult(
            subject: user.Id.ToString(),
            authenticationMethod: "pwd",
            claims: GetUserClaims(user));
        return;
    }

    public static IEnumerable<Claim> GetUserClaims(User user)
    {
        return new List<Claim>
        {
            new Claim(JwtClaimTypes.Id, user.Id.ToString()),
            new Claim(JwtClaimTypes.Name, user.FullName),
            new Claim(JwtClaimTypes.Email, user.Email),
            new Claim(JwtClaimTypes.Role, user.Role.ToString())
        };
    }
}
