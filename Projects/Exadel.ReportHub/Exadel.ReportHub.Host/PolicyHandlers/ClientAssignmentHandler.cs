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

    public ClientAssignmentRequirement(IList<UserRole> roles)
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
        if (!context.User.IsAuthenticated())
        {
            return;
        }

        var containedRoles = requirement.Roles.Where(r => context.User.IsInRole(r.ToString()));
        if (containedRoles.IsNullOrEmpty())
        {
            return;
        }

        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return;
        }

        var userId = Guid.Parse(userIdClaim.Value);
        if (containedRoles.Contains(UserRole.SuperAdmin) || containedRoles.Contains(UserRole.Regular))
        {
            if (!await _userAssignmentRepository.ExistsAsync(userId, Handlers.Constants.Client.GlobalId, CancellationToken.None))
            {
                return;
            }

            context.Succeed(requirement);
            return;
        }

        var clientId = await GetClientIdFromRequestAsync(_httpContextAccessor.HttpContext.Request);

        if (clientId == Guid.Empty || !await _userAssignmentRepository.ExistsAsync(userId, clientId, CancellationToken.None))
        {
            return;
        }

        var role = await _userAssignmentRepository.GetRoleAsync(userId, clientId, CancellationToken.None);

        if (containedRoles.Contains(role))
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
