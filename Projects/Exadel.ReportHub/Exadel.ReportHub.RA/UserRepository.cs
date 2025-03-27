using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.RA.Interfaces;
using MongoDB.Driver;

namespace Exadel.ReportHub.RA;
public class UserRepository : BaseRepository<User>, IUserRepository
{
    private static readonly FilterDefinitionBuilder<User> _filterBuilder = Builders<User>.Filter;

    public UserRepository(MongoDbContext context)
        : base(context)
    {
    }

    public async Task AddUserAsync(User user, CancellationToken cancellationToken)
    {
        await AddAsync(user, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllActiveAsync(CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.Eq(x => x.IsActive, true);
        return await GetAsync(filter, cancellationToken);
    }

    public async Task UpdateActivityAsync(Guid id, bool isActive, CancellationToken cancellationToken)
    {
        var update = Builders<User>.Update.Set(x => x.IsActive, isActive);
        await UpdateAsync(id, update, cancellationToken);
    }

    public async Task<bool> IsActiveAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.Eq(x => x.Id, id);
        var user = await GetCollection().Find(filter).Project(x => x.IsActive).SingleOrDefaultAsync(cancellationToken);
        return user;
    }
}
