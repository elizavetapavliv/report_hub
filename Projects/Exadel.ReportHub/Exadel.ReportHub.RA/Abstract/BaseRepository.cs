using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Data.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Exadel.ReportHub.RA.Abstract;

[ExcludeFromCodeCoverage]
public abstract class BaseRepository(MongoDbContext context)
{
    public async Task<IList<TDocument>> GetAllAsync<TDocument>(CancellationToken cancellationToken)
    {
        var filter = Builders<TDocument>.Filter.Empty;

        return await GetCollection<TDocument>().Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<IList<TDocument>> GetAsync<TDocument>(FilterDefinition<TDocument> filter, CancellationToken cancellationToken)
    {
        return await GetCollection<TDocument>().Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<TDocument> GetByIdAsync<TDocument>(Guid id, CancellationToken cancellationToken)
        where TDocument : IDocument
    {
        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
        WithSoftDeleteAndActive(ref filter);

        return await GetCollection<TDocument>().Find(filter).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IList<TDocument>> GetByIdsAsync<TDocument>(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        where TDocument : IDocument
    {
        var filter = Builders<TDocument>.Filter.In(x => x.Id, ids);
        WithSoftDeleteAndActive(ref filter);

        return await GetAsync(filter, cancellationToken);
    }

    public async Task<bool> ExistsAsync<TDocument>(Guid id, CancellationToken cancellationToken)
        where TDocument : IDocument
    {
        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
        WithSoftDeleteAndActive(ref filter);

        var count = await GetCollection<TDocument>().CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        return count > 0;
    }

    public async Task AddAsync<TDocument>(TDocument entity, CancellationToken cancellationToken)
    {
        await GetCollection<TDocument>().InsertOneAsync(entity, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync<TDocument>(Guid id, TDocument entity, CancellationToken cancellationToken)
        where TDocument : IDocument
    {
        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);

        await GetCollection<TDocument>().ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync<TDocument>(Guid id, UpdateDefinition<TDocument> update, CancellationToken cancellationToken)
        where TDocument : IDocument
    {
        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);

        await GetCollection<TDocument>().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync<TDocument>(Guid id, CancellationToken cancellationToken)
        where TDocument : IDocument
    {
        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);

        await GetCollection<TDocument>().DeleteOneAsync(filter, cancellationToken: cancellationToken);
    }

    public async Task SoftDeleteAsync<TDocument>(Guid id, CancellationToken cancellationToken)
        where TDocument : IDocument, ISoftDeletable
    {
        var update = Builders<TDocument>.Update.Set(x => x.IsDeleted, true);

        await UpdateAsync(id, update, cancellationToken: cancellationToken);
    }

    public IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName = null)
    {
        return context.GetCollection<TDocument>(collectionName);
    }

    public async Task AddManyAsync<TDocument>(IEnumerable<TDocument> entities, CancellationToken cancellationToken)
    {
        await GetCollection<TDocument>().InsertManyAsync(entities, cancellationToken: cancellationToken);
    }

    public static void WithSoftDeleteAndActive<TDocument>(ref FilterDefinition<TDocument> filter)
    {
        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TDocument)))
        {
            filter &= Builders<TDocument>.Filter.Eq(nameof(ISoftDeletable.IsDeleted), BsonValue.Create(false));
        }

        if (typeof(IActivable).IsAssignableFrom(typeof(TDocument)))
        {
            filter &= Builders<TDocument>.Filter.Eq(nameof(IActivable.IsActive), BsonValue.Create(true));
        }
    }
}
