using AutoMapper;
using Exadel.ReportHub.Data.Abstract;
using Exadel.ReportHub.RA;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Client;

namespace Exadel.ReportHub.Handlers.Managers.Common;

public class CountryBasedEntityManager<TDto, TEntity>(
    IMapper mapper,
    ICountryRepository countryRepository) : ICountryBasedEntityManager<TDto, TEntity>
    where TDto : new()
    where TEntity : IDocument, ICountryBasedDocument
{
    public async Task<TEntity> GenerateEntityAsync(TDto entityDto, CancellationToken cancellationToken)
    {
        var entity = (await GenerateEntitiesAsync([entityDto], cancellationToken)).Single();

        return entity;
    }

    public async Task<IList<TEntity>> GenerateEntitiesAsync(IEnumerable<TDto> entityDtos, CancellationToken cancellationToken)
    {
        var entities = mapper.Map<IList<TEntity>>(entityDtos);

        var countryTask = countryRepository.GetByIdsAsync(entities.Select(x => x.CountryId).Distinct(), cancellationToken);

        await Task.WhenAll(countryTask);
        var countries = countryTask.Result.ToDictionary(x => x.Id);

        foreach (var entity in entities)
        {
            countries.TryGetValue(entity.CountryId, out var country);

            entity.Country = country.Name;
            entity.CurrencyId = country.CurrencyId;
            entity.CurrencyCode = country.CurrencyCode;
            entity.Id = Guid.NewGuid();
        }

        return entities;
    }
}
