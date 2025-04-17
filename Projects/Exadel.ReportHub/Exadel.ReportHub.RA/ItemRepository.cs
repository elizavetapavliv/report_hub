using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;

namespace Exadel.ReportHub.RA;

public class ItemRepository : BaseRepository, IItemRepository
{
    public ItemRepository(MongoDbContext context)
        : base(context)
    {
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return ExistsAsync<Item>(id, cancellationToken);
    }
}
