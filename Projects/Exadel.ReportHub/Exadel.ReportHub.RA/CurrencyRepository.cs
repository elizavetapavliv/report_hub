using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;

namespace Exadel.ReportHub.RA;

public class CurrencyRepository : BaseRepository, ICurrencyRepository
{
    public CurrencyRepository(MongoDbContext context)
        : base(context)
    {
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await ExistsAsync<Currency>(id, cancellationToken);
    }

    public async Task<Currency> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await GetByIdAsync<Currency>(id, cancellationToken);
    }
}
