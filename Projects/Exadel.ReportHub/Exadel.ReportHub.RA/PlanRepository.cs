﻿using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;
using MongoDB.Driver;

namespace Exadel.ReportHub.RA;

public class PlanRepository : BaseRepository, IPlanRepository
{
    private static readonly FilterDefinitionBuilder<Plan> _filterBuilder = Builders<Plan>.Filter;

    public PlanRepository(MongoDbContext context)
        : base(context)
    {
    }

    public async Task AddAsync(Plan plan, CancellationToken cancellationToken)
    {
        await AddAsync<Plan>(plan, cancellationToken);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var update = Builders<Plan>.Update.Set(x => x.IsDeleted, true);
        await UpdateAsync(id, update, cancellationToken);
    }

    public Task<IEnumerable<Plan>> GetAsync(CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.Eq(x => x.IsDeleted, false);
        return GetAsync(filter, cancellationToken);
    }

    public Task<Plan> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return GetByIdAsync<Plan>(id, cancellationToken);
    }

    public async Task UpdateAmountAsync(Guid id, int amount, CancellationToken cancellationToken)
    {
        var update = Builders<Plan>.Update.Set(x => x.Amount, amount);
        await UpdateAsync(id, update, cancellationToken);
    }

    public async Task UpdateDateAsync(Guid id, Plan plan, CancellationToken cancellationToken)
    {
        var update = Builders<Plan>.Update
            .Set(x => x.StartDate, plan.StartDate)
            .Set(x => x.EndDate, plan.EndDate);
        await UpdateAsync(id, update, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid itemId, Guid clientId, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.And(
            _filterBuilder.Eq(x => x.ClientId, clientId),
            _filterBuilder.Eq(x => x.ItemId, itemId),
            _filterBuilder.Eq(x => x.IsDeleted, false));
        var count = await GetCollection<Plan>().CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        return count > 0;
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return ExistsAsync<Plan>(id, cancellationToken);
    }
}
