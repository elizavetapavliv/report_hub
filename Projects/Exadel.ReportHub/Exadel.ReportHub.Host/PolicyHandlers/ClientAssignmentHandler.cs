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
    public List<UserRole> Roles { get; }

    public ClientAssignmentRequirement(params UserRole[] roles)
    {
        Roles = roles.ToList();
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

        var matchingRoles = requirement.Roles.Where(r => context.User.IsInRole(r.ToString()));
        if (matchingRoles.IsNullOrEmpty())
        {
            return;
        }

        var userId = Guid.Parse(userIdClaim.Value);
        var clientId = await GetClientIdFromRequestAsync(_httpContextAccessor.HttpContext.Request);
        if (clientId == Guid.Empty)
        {
            return;
        }

        var role = await _userAssignmentRepository.GetRoleAsync(userId, clientId, CancellationToken.None);
        if (role != null && matchingRoles.Contains(role.Value))
        {
            context.Succeed(requirement);
            return;
        }

        role = await _userAssignmentRepository.GetRoleAsync(userId, Handlers.Constants.Client.GlobalId, CancellationToken.None);
        if (role != null && matchingRoles.Contains(role.Value))
        {
            context.Succeed(requirement);
        }
    }

    private async Task<Guid> GetClientIdFromRequestAsync(HttpRequest request)
    {
        var clientId = Guid.Empty;

        if (request.RouteValues.TryGetValue("controller", out var serviceName) &&
            serviceName.ToString().Equals(typeof(ClientService).Name, StringComparison.Ordinal) &&
            request.RouteValues.TryGetValue("id", out var routeClientId) &&
            Guid.TryParse(routeClientId.ToString(), out clientId))
        {
            return clientId;
        }

        if (request.Query.TryGetValue("clientId", out var queryClientId) &&
            Guid.TryParse(queryClientId.ToString(), out clientId))
        {
            return clientId;
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

        return clientId;
    }

    private sealed record ClientIdContainer(Guid ClientId);
}
