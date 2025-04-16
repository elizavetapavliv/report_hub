using Exadel.ReportHub.Data.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Exadel.ReportHub.RA;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var mongoDbSettings = configuration.GetConnectionString("Mongo");
        var client = new MongoClient(mongoDbSettings);
        _database = client.GetDatabase("ReportHub");
        TtlIndexForExcnageRate();
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName = null)
    {
        return _database.GetCollection<T>(collectionName ?? typeof(T).Name);
    }

    private void TtlIndexForExcnageRate()
    {
        var collection = _database.GetCollection<ExchangeRate>("ExchangeRate");

        var indexKeysDefinition = Builders<ExchangeRate>.IndexKeys.Ascending(x => x.Date);
        var indexOptions = new CreateIndexOptions { ExpireAfter = new TimeSpan(24, 0, 0) };
        var indexModel = new CreateIndexModel<ExchangeRate>(indexKeysDefinition, indexOptions);

        collection.Indexes.CreateOne(indexModel);
    }
}
