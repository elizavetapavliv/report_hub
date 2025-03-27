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
        var filter = Builders<User>.Filter.Eq(x => x.IsActive, true);
        return await GetCollection().Find(filter).ToListAsync(cancellationToken);
    }

    public async Task UpdateActivityAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Id, id);
        var update = Builders<User>.Update.Set(x => x.IsActive, false);
        await GetCollection().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task<bool> IsActiveByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Id, id);
        var user = await GetCollection().Find(filter).SingleOrDefaultAsync(cancellationToken);
        return user?.IsActive ?? false;
    }
}
