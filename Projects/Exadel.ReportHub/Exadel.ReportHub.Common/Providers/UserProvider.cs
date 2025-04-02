using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.ReportHub.Common.Providers;

public class UserProvider(IHttpContextAccessor httpContextAccessor) : IUserProvider
{
    public Guid GetUserId()
    {
        var user = httpContextAccessor.HttpContext?.User;
        var userClaim = user?.FindFirst("sub")?.Value;


        if (string.IsNullOrEmpty(userClaim))
        {
            throw new HttpStatusCodeException(new List<string> { "User ID not found" }, HttpStatusCode.Unauthorized);
        }

        return Guid.Parse(userClaim);
    }
}
