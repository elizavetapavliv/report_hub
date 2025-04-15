using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;
using MongoDB.Driver;

namespace Exadel.ReportHub.RA;

[ExcludeFromCodeCoverage]
public class CustomerRepository : BaseRepository, ICustomerRepository
{
    private static readonly FilterDefinitionBuilder<Customer> _filterBuilder = Builders<Customer>.Filter;

    public CustomerRepository(MongoDbContext context)
        : base(context)
    {
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
    {
        await AddAsync<Customer>(customer, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.Eq(x => x.Email, email);
        var count = await GetCollection<Customer>().Find(filter).CountDocumentsAsync(cancellationToken);
        return count > 0;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await ExistsAsync<Customer>(id, cancellationToken);
    }

    public Task<IEnumerable<Customer>> GetAsync(bool? isDeleted, CancellationToken cancellationToken)
    {
        var filter = isDeleted.HasValue
            ? _filterBuilder.Eq(x => x.IsDeleted, isDeleted.Value)
            : _filterBuilder.Empty;

        return GetAsync(filter, cancellationToken);
    }

    public Task<Customer> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return GetByIdAsync<Customer>(id, cancellationToken);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var definition = Builders<Customer>.Update.Set(x => x.IsDeleted, true);
        await UpdateAsync(id, definition, cancellationToken);
    }

    public async Task UpdateNameAsync(Guid id, string name, CancellationToken cancellationToken)
    {
        var definition = Builders<Customer>.Update.Set(x => x.Name, name);
        await UpdateAsync(id, definition, cancellationToken);
    }
}
