﻿using Exadel.ReportHub.Data.Enums;
using Exadel.ReportHub.Data.Models;

namespace Exadel.ReportHub.RA.Abstract;

public interface IUserAssignmentRepository
{
    Task SetRoleAsync(UserAssignment userAssignment, CancellationToken cancellationToken);

    Task<IEnumerable<UserRole>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(Guid userId, Guid clientId, CancellationToken cancellationToken);

    Task<bool> ExistAnyAsync(Guid userId, IEnumerable<Guid> clientIds, IEnumerable<UserRole> roles, CancellationToken cancellationToken);
}
