using Exadel.ReportHub.Data.Enums;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;
using MongoDB.Driver;

namespace Exadel.ReportHub.RA;

public class UserAssignmentRepository : BaseRepository, IUserAssignmentRepository
{
    private static readonly FilterDefinitionBuilder<UserAssignment> _filterBuilder = Builders<UserAssignment>.Filter;

    public UserAssignmentRepository(MongoDbContext context)
        : base(context)
    {
    }

    public async Task AddAsync(UserAssignment userAssignment, CancellationToken cancellationToken)
    {
        await base.AddAsync(userAssignment, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid clientId, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.And(_filterBuilder.Eq(x => x.UserId, userId), _filterBuilder.Eq(x => x.ClientId, clientId));
        var count = await GetCollection<UserAssignment>().Find(filter).CountDocumentsAsync(cancellationToken);
        return count > 0;
    }

    public async Task<UserRole> GetRoleAsync(Guid userId, Guid clientId, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.And(_filterBuilder.Eq(x => x.UserId, userId), _filterBuilder.Eq(x => x.ClientId, clientId));
        var userAssignment = await GetCollection<UserAssignment>().Find(filter).SingleOrDefaultAsync(cancellationToken);
        return userAssignment.Role;
    }

    public async Task UpdateRoleAsync(Guid userId, Guid clientId, UserRole userRole, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.And(_filterBuilder.Eq(x => x.UserId, userId), _filterBuilder.Eq(x => x.ClientId, clientId));
        var update = Builders<UserAssignment>.Update.Set(x => x.Role, userRole);
        await GetCollection<UserAssignment>().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }
}
