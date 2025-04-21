using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;

namespace Exadel.ReportHub.RA;

[ExcludeFromCodeCoverage]
public class InvoiceRepository : BaseRepository, IInvoiceRepository
{
    public InvoiceRepository(MongoDbContext context)
        : base(context)
    {
    }

    public async Task AddManyAsync(IEnumerable<Invoice> invoices, CancellationToken cancellationToken)
    {
        await base.AddManyAsync(invoices, cancellationToken);
    }

    public async Task<Invoice> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await GetByIdAsync<Invoice>(id, cancellationToken);
    }
}
