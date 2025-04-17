using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;
using MongoDB.Driver;

namespace Exadel.ReportHub.RA;

public class ItemRepository : BaseRepository, IItemRepository
{
    public ItemRepository(MongoDbContext context)
        : base(context)
    {
    }

    public async Task AddAsync(Item item, CancellationToken cancellationToken)
    {
        await base.AddAsync(item, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await ExistsAsync<Item>(id, cancellationToken);
    }

    public async Task<Item> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await GetByIdAsync<Item>(id, cancellationToken);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await SoftDeleteAsync<Item>(id, cancellationToken);
    }

    public async Task UpdateAsync(Item item, CancellationToken cancellationToken)
    {
        await UpdateAsync(item.Id, item, cancellationToken);
    }
}
