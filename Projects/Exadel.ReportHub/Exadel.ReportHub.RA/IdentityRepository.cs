using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;

namespace Exadel.ReportHub.RA
{
    public class IdentityRepository<TDocument> : BaseRepository<TDocument>, IIdentityRepository<TDocument>
        where TDocument : IDocument
    {
        public IdentityRepository(MongoDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<TDocument>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await base.GetAllAsync(cancellationToken);
        }

        public async Task<TDocument> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await base.GetByIdAsync(id, cancellationToken);
        }
    }
}
