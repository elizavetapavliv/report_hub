﻿using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Data.Models;
using MongoDB.Driver;

namespace Exadel.ReportHub.RA.Abstract;

[ExcludeFromCodeCoverage]
public abstract class BaseRepository(MongoDbContext context)
{
    public async Task<IEnumerable<TDocument>> GetAllAsync<TDocument>(CancellationToken cancellationToken)
    {
        var filter = Builders<TDocument>.Filter.Empty;
        return await GetCollection<TDocument>().Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TDocument>> GetAsync<TDocument>(FilterDefinition<TDocument> filter, CancellationToken cancellationToken)
    {
        return await GetCollection<TDocument>().Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<TDocument> GetByIdAsync<TDocument>(Guid id, CancellationToken cancellationToken)
        where TDocument : IDocument
    {
        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
        return await GetCollection<TDocument>().Find(filter).SingleOrDefaultAsync(cancellationToken);
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

    public IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName = null)
    {
        return context.GetCollection<TDocument>(collectionName);
    }
}
