﻿using Exadel.ReportHub.Data.Models;

namespace Exadel.ReportHub.RA.Abstract;

public interface IInvoiceRepository
{
    Task AddManyAsync(IEnumerable<Invoice> invoices, CancellationToken cancellationToken);

    Task AddAsync(Invoice invoice, CancellationToken cancellationToken);

    Task<IList<Invoice>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken);

    Task<Invoice> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(string invoiceNumber, CancellationToken cancellationToken);

    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken);

    Task UpdateAsync(Invoice invoice, CancellationToken cancellationToken);

    Task<IList<Invoice>> GetByDateRangeAsync(Invoice invoice, CancellationToken cancellationToken);
}
