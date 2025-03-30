using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exadel.ReportHub.Data.Models;
using MongoDB.Driver;

namespace Exadel.ReportHub.RA.Abstract;

public interface IBaseRepository<TDocument>
    where TDocument : IDocument
{
    Task<IEnumerable<TDocument>> GetAllAsync(CancellationToken cancellationToken);

    Task<IEnumerable<TDocument>> GetAsync(FilterDefinition<TDocument> filter, CancellationToken cancellationToken);

    Task<TDocument> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task AddAsync(TDocument entity, CancellationToken cancellationToken);

    Task UpdateAsync(Guid id, TDocument entity, CancellationToken cancellationToken);

    Task UpdateAsync(Guid id, UpdateDefinition<TDocument> update, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
