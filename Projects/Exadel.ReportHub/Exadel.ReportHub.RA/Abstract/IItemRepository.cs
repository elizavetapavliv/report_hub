﻿using Exadel.ReportHub.Data.Models;

namespace Exadel.ReportHub.RA.Abstract;

public interface IItemRepository
{
    Task AddAsync(Item item, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);

    Task<Item> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task UpdatePriceAsync(Guid id, decimal price, CancellationToken cancellationToken);

    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken);
}
