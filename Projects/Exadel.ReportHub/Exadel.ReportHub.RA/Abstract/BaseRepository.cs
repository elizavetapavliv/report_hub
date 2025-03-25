using Exadel.ReportHub.Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Exadel.ReportHub.RA.Abstract;
public class BaseRepository<TDocument>
    where TDocument : IDocument
{
    private readonly IMongoCollection<TDocument> _collection;
    private static readonly FilterDefinitionBuilder<TDocument> _filterBuilder = Builders<TDocument>.Filter;

    public BaseRepository(MongoDbContext context)
    {
        _collection = context.GetCollection<TDocument>();
    }

    public async Task<IEnumerable<TDocument>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _collection.Find(_filterBuilder.Empty).ToListAsync();
    }

    public async Task<TDocument> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(_filterBuilder.Eq(x => x.Id, id)).FirstOrDefaultAsync();
    }

    public async Task AddAsync(TDocument entity, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(Guid id, TDocument entity, CancellationToken cancellationToken = default)
    {
        await _collection.ReplaceOneAsync(_filterBuilder.Eq(x => x.Id, id), entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _collection.DeleteOneAsync(_filterBuilder.Eq(x => x.Id, id));
    }
}
