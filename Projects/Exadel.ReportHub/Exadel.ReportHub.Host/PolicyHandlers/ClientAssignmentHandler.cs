using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer.Extensions;
using Exadel.ReportHub.Data.Enums;
using Exadel.ReportHub.Host.Services;
using Exadel.ReportHub.RA.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace Exadel.ReportHub.Host.PolicyHandlers;

public class ClientAssignmentRequirement : IAuthorizationRequirement
{
    public IList<UserRole> Roles { get; }

    public ClientAssignmentRequirement(params UserRole[] roles)
    {
        Roles = roles;
        if (!Roles.Contains(UserRole.SuperAdmin))
        {
            Roles.Add(UserRole.SuperAdmin);
        }
    }
}

public class ClientAssignmentHandler : AuthorizationHandler<ClientAssignmentRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserAssignmentRepository _userAssignmentRepository;

    public ClientAssignmentHandler(IHttpContextAccessor httpContextAccessor, IUserAssignmentRepository userAssignmentRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userAssignmentRepository = userAssignmentRepository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClientAssignmentRequirement requirement)
    {
        Claim userIdClaim;
        if (!context.User.IsAuthenticated() ||
            (userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)) == null)
        {
            return;
        }

        var matchingRoles = requirement.Roles.Where(r => context.User.IsInRole(r.ToString())).ToList();
        if (matchingRoles.Count == 0)
        {
            return;
        }

        var userId = Guid.Parse(userIdClaim.Value);
        var requiredClientIds = new List<Guid> { Handlers.Constants.Client.GlobalId };
        var requestClientId = await GetClientIdFromRequestAsync(_httpContextAccessor.HttpContext.Request);

        if (requestClientId != null)
        {
            requiredClientIds.Add(requestClientId.Value);
        }

        var assignedClientIds = await _userAssignmentRepository.GetClientIdsByRolesAsync(userId, matchingRoles, CancellationToken.None);

        if (requiredClientIds.Intersect(assignedClientIds).Any())
        {
                context.Succeed(requirement);
        }
    }

    private async Task<Guid?> GetClientIdFromRequestAsync(HttpRequest request)
    {
        if (request.RouteValues.TryGetValue("controller", out var serviceName) &&
            serviceName.ToString().Equals(typeof(ClientService).Name, StringComparison.Ordinal) &&
            request.RouteValues.TryGetValue("id", out var routeClientIdObj) &&
            Guid.TryParse(routeClientIdObj.ToString(), out var routeClientId))
        {
            return routeClientId;
        }

        if (request.Query.TryGetValue("clientId", out var queryClientIdObj) &&
            Guid.TryParse(queryClientIdObj.ToString(), out var queryClientId))
        {
            return queryClientId;
        }

        if (request.ContentType?.Contains("application/json", StringComparison.Ordinal) == true)
        {
            request.EnableBuffering();

            var container = await JsonSerializer.DeserializeAsync<ClientIdContainer>(
                request.BodyReader.AsStream(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                request.HttpContext.RequestAborted);
            request.Body.Position = 0;

            if (container != null)
            {
                return container.ClientId;
            }
        }

        return null;
    }

    private sealed record ClientIdContainer(Guid ClientId);
}
