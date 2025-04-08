using System.Security.Claims;
using Exadel.ReportHub.Data.Enums;
using Exadel.ReportHub.RA.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace Exadel.ReportHub.Host.PolicyHandlers;

public class ClientAdminRequirement : IAuthorizationRequirement
{
}

public class ClientAdminHandler : AuthorizationHandler<ClientAdminRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserAssignmentRepository _userAssignmentRepository;

    public ClientAdminHandler(IHttpContextAccessor httpContextAccessor, IUserAssignmentRepository userAssignmentRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userAssignmentRepository = userAssignmentRepository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClientAdminRequirement requirement)
    {
        if (context.User.IsInRole(UserRole.SuperAdmin.ToString()))
        {
            context.Succeed(requirement);
            return;
        }

        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return;
        }

        var routeData = _httpContextAccessor.HttpContext.GetRouteData();
        if (!routeData.Values.TryGetValue("clientId", out var clientIdObj))
        {
            return;
        }

        var userId = Guid.Parse(userIdClaim.Value);
        var clientId = Guid.Parse(clientIdObj.ToString());
        if (!await _userAssignmentRepository.ExistsAsync(userId, clientId, CancellationToken.None))
        {
            return;
        }

        var role = await _userAssignmentRepository.GetRoleAsync(userId, clientId, CancellationToken.None);

        if (role == UserRole.ClientAdmin)
        {
            context.Succeed(requirement);
        }
    }
}
