﻿using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;
using MongoDB.Driver;

namespace Exadel.ReportHub.RA;

public class ClientRepository : BaseRepository, IClientRepository
{
    private static readonly FilterDefinitionBuilder<Client> _filterBuilder = Builders<Client>.Filter;

    public ClientRepository(MongoDbContext context)
        : base(context)
    {
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.Eq(x => x.Id, id);
        var count = await GetCollection<Client>().Find(filter).CountDocumentsAsync(cancellationToken);
        return count > 0;
    }
}
