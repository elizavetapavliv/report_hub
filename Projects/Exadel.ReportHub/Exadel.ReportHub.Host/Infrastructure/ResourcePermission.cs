using Exadel.ReportHub.Data.Enums;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.Host.Infrastructure.Enums;

namespace Exadel.ReportHub.Host.Infrastructure;

public static class ResourcePermission
{
    public static Dictionary<UserRole, List<Permission>> GetPermissions(string resource)
    {
        return resource switch
        {
            nameof(Client) => new()
            {
                { UserRole.SuperAdmin, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.Owner, new() { Permission.Read, Permission.Update } },
                { UserRole.ClientAdmin, new() { Permission.Read } },
                { UserRole.Operator, new() { Permission.Read } }
            },
            nameof(User) => new()
            {
                { UserRole.SuperAdmin, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.Owner, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.ClientAdmin, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.Operator, new() { Permission.Read } }
            },
            nameof(Item) => new()
            {
                { UserRole.Owner, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.ClientAdmin, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.Operator, new() { Permission.Read } }
            },
            nameof(Invoice) => new()
            {
                { UserRole.Owner, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.ClientAdmin, new() { Permission.Create, Permission.Read, Permission.Update } },
                { UserRole.Operator, new() { Permission.Create, Permission.Read } }
            },
            nameof(Customer) => new()
            {
                { UserRole.Owner, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.ClientAdmin, new() { Permission.Create, Permission.Read, Permission.Update } },
                { UserRole.Operator, new() { Permission.Create, Permission.Read } }
            },
            _ => new()
        };
    }
}
