using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Exadel.ReportHub.RA;

[ExcludeFromCodeCoverage]
public class ExchangeRateRepository(MongoDbContext context) : BaseRepository(context), IExchangeRateRepository
{
    private static readonly FilterDefinitionBuilder<ExchangeRate> _filterBuilder = Builders<ExchangeRate>.Filter;

    public Task AddManyAsync(IEnumerable<ExchangeRate> exchangeRates, CancellationToken cancellationToken)
    {
        return base.AddManyAsync(exchangeRates, cancellationToken);
    }

    public async Task<ExchangeRate> GetByCurrencyAsync(string currency, DateTime date, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.And(
            _filterBuilder.Eq(x => x.Currency, currency),
            _filterBuilder.Eq(x => x.RateDate, date));
        return await GetCollection<ExchangeRate>().Find(filter).SingleOrDefaultAsync(cancellationToken);
    }
}
