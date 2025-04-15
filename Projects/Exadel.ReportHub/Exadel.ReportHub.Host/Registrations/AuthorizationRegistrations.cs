using Exadel.ReportHub.Data.Enums;
using Exadel.ReportHub.Host.Infrastructure.Enums;
using Exadel.ReportHub.Host.PolicyHandlers;

namespace Exadel.ReportHub.Host.Registrations;

public static class AuthorizationRegistrations
{
    public static void AddAuthorizationPolicy(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Constants.Authorization.Policy.AllUsers, policy =>
                policy.Requirements.Add(new ClientAssignmentRequirement(UserRole.Regular)));
            options.AddPolicy(Constants.Authorization.Policy.ClientAdmin, policy =>
                policy.Requirements.Add(new ClientAssignmentRequirement(UserRole.ClientAdmin)));
            options.AddPolicy(Constants.Authorization.Policy.SuperAdmin, policy =>
                policy.Requirements.Add(new ClientAssignmentRequirement()));

            options.AddPolicy(Constants.Authorization.Policy.Create, policy =>
                policy.Requirements.Add(new PermissionRequirement(Permission.Create)));
            options.AddPolicy(Constants.Authorization.Policy.Read, policy =>
                policy.Requirements.Add(new PermissionRequirement(Permission.Read)));
            options.AddPolicy(Constants.Authorization.Policy.Update, policy =>
                policy.Requirements.Add(new PermissionRequirement(Permission.Update)));
            options.AddPolicy(Constants.Authorization.Policy.Delete, policy =>
                policy.Requirements.Add(new PermissionRequirement(Permission.Delete)));
        });
    }
}
