using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;
using MongoDB.Driver;

namespace Exadel.ReportHub.RA;

[ExcludeFromCodeCoverage]
public class ExchangeRateRepository(MongoDbContext context) : BaseRepository(context), IExchangeRateRepository
{
    private static readonly FilterDefinitionBuilder<ExchangeRate> _filterBuilder = Builders<ExchangeRate>.Filter;

    public Task AddManyAsync(IEnumerable<ExchangeRate> exchangeRates, CancellationToken cancellationToken)
    {
        return base.AddManyAsync(exchangeRates, cancellationToken);
    }

    public async Task UpsertAsync(IEnumerable<ExchangeRate> exchangeRates, CancellationToken cancellationToken)
    {
        var models = exchangeRates
            .Select(exchangeRate =>
                new ReplaceOneModel<ExchangeRate>(
                    Builders<ExchangeRate>.Filter.Eq(x => x.Currency, exchangeRate.Currency),
                    exchangeRate)
                {
                    IsUpsert = true
                });
        await GetCollection<ExchangeRate>().BulkWriteAsync(models, cancellationToken: cancellationToken);
    }

    public Task<IList<ExchangeRate>> GetAllAsync(CancellationToken cancellationToken)
    {
        return GetAllAsync<ExchangeRate>(cancellationToken);
    }

    public async Task<ExchangeRate> GetByCurrencyAsync(string currency, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.Eq(x => x.Currency, currency);
        return await GetCollection<ExchangeRate>().Find(filter).SingleOrDefaultAsync(cancellationToken);
    }
}
