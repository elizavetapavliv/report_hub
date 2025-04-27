using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;
using MongoDB.Driver;

namespace Exadel.ReportHub.RA;

public class AuditReportRepository(MongoDbContext context) : BaseRepository(context), IAuditReportRepository
{
    private static readonly FilterDefinitionBuilder<AuditReport> _filterBuilder = Builders<AuditReport>.Filter;

    public async Task<AuditReport> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await GetByIdAsync<AuditReport>(id, cancellationToken);
    }

    public async Task<IList<AuditReport>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.Eq(x => x.UserId, userId);
        return await GetAsync(filter, cancellationToken);
    }

    public async Task AddAsync(AuditReport auditReport, CancellationToken cancellationToken)
    {
        await base.AddAsync(auditReport, cancellationToken);
    }
}
