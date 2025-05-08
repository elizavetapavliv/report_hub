using Exadel.ReportHub.Data.Abstract;
using Exadel.ReportHub.SDK.DTOs.Client;

namespace Exadel.ReportHub.Handlers.Managers.Common;

public interface ICountryBasedEntityManager<TDto, TEntity>
    where TDto : new()
    where TEntity : IDocument, ICountryBasedDocument
{
    Task<TEntity> GenerateEntityAsync(TDto entityDto, CancellationToken cancellationToken);

    Task<IList<TEntity>> GenerateEntitiesAsync(IEnumerable<TDto> entityDtos, CancellationToken cancellationToken);
}
