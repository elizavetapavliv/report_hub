using System.Security.Claims;
using Exadel.ReportHub.RA.Abstract;
using Microsoft.AspNetCore.Http;

namespace Exadel.ReportHub.RA;
public class UserProvider(IHttpContextAccessor httpContextAccessor) : IUserProvider
{
    public Guid GetUserID()
    {
        var user = httpContextAccessor.HttpContext?.User;
        var userClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userClaim))
        {
            throw new UnauthorizedAccessException("User ID not found");
        }

        return Guid.Parse(userClaim);
    }
}
