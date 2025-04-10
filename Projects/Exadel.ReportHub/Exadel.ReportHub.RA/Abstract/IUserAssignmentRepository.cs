using Exadel.ReportHub.Data.Enums;
using Exadel.ReportHub.Data.Models;

namespace Exadel.ReportHub.RA.Abstract;

public interface IUserAssignmentRepository
{
    Task AddAsync(UserAssignment userAssignment, CancellationToken cancellationToken);

    Task<UserRole?> GetRoleAsync(Guid userId, Guid clientId, CancellationToken cancellationToken);

    Task<IEnumerable<UserRole>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken);

    Task<IEnumerable<Guid>> GetClientIdsByRolesAsync(Guid userId, IEnumerable<UserRole> roles, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(Guid userId, Guid clientId, CancellationToken cancellationToken);

    Task UpdateRoleAsync(Guid userId, Guid clientId, UserRole userRole, CancellationToken cancellationToken);
}
