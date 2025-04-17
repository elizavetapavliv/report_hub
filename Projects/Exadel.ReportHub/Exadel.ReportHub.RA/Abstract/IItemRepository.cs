namespace Exadel.ReportHub.RA.Abstract;

public interface IItemRepository
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}
