using System.Security.Claims;
using Exadel.ReportHub.Common;
using Microsoft.AspNetCore.Http;

namespace Exadel.ReportHub.RA;
public class UserProvider(IHttpContextAccessor httpContextAccessor) : IUserProvider
{
    public Guid GetUserId()
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
