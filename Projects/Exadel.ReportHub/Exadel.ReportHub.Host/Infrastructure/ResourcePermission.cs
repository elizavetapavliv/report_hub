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
            "Clients" => new()
            {
                { UserRole.SuperAdmin, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.Owner, new() { Permission.Read, Permission.Update } },
                { UserRole.ClientAdmin, new() { Permission.Read } },
                { UserRole.Operator, new() { Permission.Read } }
            },
            "Users" => new()
            {
                { UserRole.SuperAdmin, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.Owner, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.ClientAdmin, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.Operator, new() { Permission.Read } }
            },
            "UserAssignments" => new()
            {
                { UserRole.SuperAdmin, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.Owner, new() { Permission.Create, Permission.Read } },
                { UserRole.ClientAdmin, new() { Permission.Read } },
                { UserRole.Operator, new() { Permission.Read } }
            },
            "Items" => new()
            {
                { UserRole.Owner, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.ClientAdmin, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.Operator, new() { Permission.Read } }
            },
            "Invoices" => new()
            {
                { UserRole.Owner, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.ClientAdmin, new() { Permission.Create, Permission.Read, Permission.Update } },
                { UserRole.Operator, new() { Permission.Create, Permission.Read } }
            },
            "Customers" => new()
            {
                { UserRole.Owner, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.ClientAdmin, new() { Permission.Create, Permission.Read, Permission.Update } },
                { UserRole.Operator, new() { Permission.Create, Permission.Read } }
            },
            "Plans" => new()
            {
                { UserRole.Owner, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.ClientAdmin, new() { Permission.Create, Permission.Read, Permission.Update } },
            },
            "Reports" => new()
            {
                { UserRole.Owner, new() { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
                { UserRole.ClientAdmin, new() { Permission.Create, Permission.Read } },
            },
            _ => new()
        };
    }
}
