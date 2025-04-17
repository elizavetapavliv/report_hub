using Exadel.ReportHub.Data.Models;

namespace Exadel.ReportHub.RA.Abstract;

public interface ICurrencyRepository
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);

    Task<Currency> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
