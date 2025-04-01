﻿using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;
using MongoDB.Driver;

namespace Exadel.ReportHub.RA;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(MongoDbContext context)
        : base(context)
    {
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Email, email);
        var count = await GetCollection<User>().Find(filter).CountDocumentsAsync(cancellationToken);
        return count > 0;
    }

    public async Task<IEnumerable<User>> GetAllActiveAsync(CancellationToken cancellationToken)
    {
        var filter = Builders<User>.Filter.Eq(x => x.IsActive, true);
        return await GetAsync(filter, cancellationToken);
    }

    public async Task UpdateActivityAsync(Guid id, bool isActive, CancellationToken cancellationToken)
    {
        var update = Builders<User>.Update.Set(x => x.IsActive, isActive);
        await UpdateAsync(id, update, cancellationToken);
    }

    public async Task<bool> IsActiveAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Id, id);
        var isActive = await GetCollection<User>().Find(filter).Project(x => x.IsActive).SingleOrDefaultAsync(cancellationToken);
        return isActive;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Id, id);
        var count = await GetCollection<User>().Find(filter).CountDocumentsAsync(cancellationToken);
        return count > 0;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await AddAsync<User>(user, cancellationToken);
    }

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await GetByIdAsync<User>(id, cancellationToken);
    }
}
