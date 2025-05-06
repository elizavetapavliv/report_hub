using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;
using MongoDB.Driver;

namespace Exadel.ReportHub.RA;

[ExcludeFromCodeCoverage]
public class InvoiceRepository(MongoDbContext context) : BaseRepository(context), IInvoiceRepository
{
    private static readonly FilterDefinitionBuilder<Invoice> _filterBuilder = Builders<Invoice>.Filter;

    public Task AddAsync(Invoice invoice, CancellationToken cancellationToken)
    {
        return base.AddAsync(invoice, cancellationToken);
    }

    public Task AddManyAsync(IEnumerable<Invoice> invoices, CancellationToken cancellationToken)
    {
        return base.AddManyAsync(invoices, cancellationToken);
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return ExistsAsync<Invoice>(id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(string invoiceNumber, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.Eq(x => x.InvoiceNumber, invoiceNumber);
        var count = await GetCollection<Invoice>().Find(filter).CountDocumentsAsync(cancellationToken);
        return count > 0;
    }

    public Task<IList<Invoice>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.And(
            _filterBuilder.Eq(x => x.ClientId, clientId),
            _filterBuilder.Eq(x => x.IsDeleted, false));
        return GetAsync(filter, cancellationToken);
    }

    public Task<Invoice> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return GetByIdAsync<Invoice>(id, cancellationToken);
    }

    public Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return SoftDeleteAsync<Invoice>(id, cancellationToken);
    }

    public Task UpdateAsync(Invoice invoice, CancellationToken cancellationToken)
    {
        var definition = Builders<Invoice>.Update
            .Set(x => x.IssueDate, invoice.IssueDate)
            .Set(x => x.DueDate, invoice.DueDate);
        return UpdateAsync(invoice.Id, definition, cancellationToken);
    }

    public async Task<(string CurrencyCode, decimal Total)> GetTotalAmountByDateRangeAsync(Guid clientId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.And(
            _filterBuilder.Gte(x => x.IssueDate, startDate),
            _filterBuilder.Lte(x => x.IssueDate, endDate),
            _filterBuilder.Eq(x => x.ClientId, clientId),
            _filterBuilder.Eq(x => x.IsDeleted, false));

        var result = await GetCollection<Invoice>()
            .Aggregate()
            .Match(filter)
            .Group(x => x.ClientCurrencyCode, g => new
            {
                Currency = g.Key,
                Total = g.Sum(x => x.ClientCurrencyAmount)
            })
            .SingleOrDefaultAsync(cancellationToken);

        return (result.Currency, result.Total);
    }

    public async Task<Dictionary<Guid, int>> GetCountByDateRangeAsync(DateTime startDate, DateTime endDate, Guid clientId, Guid? customerId, CancellationToken cancellationToken)
    {
        var filters = new List<FilterDefinition<Invoice>>
        {
            _filterBuilder.Eq(x => x.ClientId, clientId),
            _filterBuilder.Gte(x => x.IssueDate, startDate),
            _filterBuilder.Lte(x => x.IssueDate, endDate),
            _filterBuilder.Eq(x => x.IsDeleted, false)
        };
        if (customerId.HasValue)
        {
            filters.Add(_filterBuilder.Eq(x => x.CustomerId, customerId));
        }

        var filter = _filterBuilder.And(filters);
        var grouping = await GetCollection<Invoice>().Aggregate().Match(filter).Group(x => x.CustomerId, g => new { CustomerId = g.Key, Count = g.Count() }).ToListAsync(cancellationToken);
        return grouping.ToDictionary(x => x.CustomerId, x => x.Count);
    }

    public async Task<Dictionary<Guid, int>> GetClientItemsCountAsync(Guid clientId, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.Eq(x => x.ClientId, clientId);
        var grouping = await GetCollection<Invoice>().Aggregate().Match(filter).Unwind<Invoice, UnwoundInvoice>(x => x.ItemIds)
            .Group(x => x.ItemIds, g => new { ItemId = g.Key, Count = g.Count() }).ToListAsync(cancellationToken);
        return grouping.ToDictionary(x => x.ItemId, x => x.Count);
    }

    public async Task<Dictionary<(int Year, int Month), List<Invoice>>> GetGroupedByMonthAsync(Guid clientId, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.Eq(x => x.ClientId, clientId);
        var grouping = await GetCollection<Invoice>().Aggregate().Match(filter)
            .Group(x => new { x.IssueDate.Year, x.IssueDate.Month },
            g => new { g.Key.Year, g.Key.Month, Invoices = g.ToList() }).ToListAsync(cancellationToken);
        return grouping.ToDictionary(x => (x.Year, x.Month), x => x.Invoices);
    }

    public async Task<InvoicesReport> GetReportAsync(Guid clientId, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.Eq(x => x.ClientId, clientId);
        var report = await GetCollection<Invoice>().Aggregate().Match(filter)
            .Group(x => new { x.IssueDate.Year, x.IssueDate.Month }, g => new
            {
                Count = g.Count(),
                Amount = g.Sum(x => x.ClientCurrencyAmount),
                Invoices = g.ToList(),
                Currency = g.FirstOrDefault().ClientCurrencyCode
            })
            .Group(_ => true, g => new InvoicesReport
            {
                TotalCount = g.Sum(x => x.Count),
                AverageMonthCount = (int)Math.Round(g.Average(x => x.Count)),
                TotalAmount = g.Sum(x => x.Amount),
                AverageAmount = g.Sum(x => x.Amount) / g.Sum(x => x.Count),
                Currency = g.FirstOrDefault().Currency,
                UnpaidCount = g.Sum(x => x.Invoices.Count(i => i.PaymentStatus == Data.Enums.PaymentStatus.Unpaid)),
                OverdueCount = g.Sum(x => x.Invoices.Count(i => i.PaymentStatus == Data.Enums.PaymentStatus.Overdue)),
                PaidOnTimeCount = g.Sum(x => x.Invoices.Count(i => i.PaymentStatus == Data.Enums.PaymentStatus.PaidOnTime)),
                PaidLateCount = g.Sum(x => x.Invoices.Count(i => i.PaymentStatus == Data.Enums.PaymentStatus.PaidLate)),
                ReportDate = DateTime.UtcNow
            }).FirstOrDefaultAsync(cancellationToken);
        return report ?? new InvoicesReport { ReportDate = DateTime.UtcNow };
    }

    private sealed record UnwoundInvoice(Guid ItemIds);
}
