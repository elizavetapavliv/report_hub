using Duende.IdentityModel;
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

        var routeData = _httpContextAccessor.HttpContext.GetRouteData();

        if (!routeData.Values.TryGetValue("clientId", out var clientIdObj))
        {
            context.Fail(new AuthorizationFailureReason(this, "Client Id was not found."));
            return;
        }

        var userId = Guid.Parse(context.User.FindFirst(JwtClaimTypes.Subject).Value);
        var clientId = Guid.Parse(clientIdObj.ToString());
        var role = await _userAssignmentRepository.GetRoleAsync(userId, clientId, CancellationToken.None);

        if (role == UserRole.ClientAdmin)
        {
            context.Succeed(requirement);
        }
    }
}
