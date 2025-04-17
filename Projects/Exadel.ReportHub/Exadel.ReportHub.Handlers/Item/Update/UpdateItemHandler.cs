using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Item;
using MediatR;

namespace Exadel.ReportHub.Handlers.Item.Update;

public record UpdateItemRequest(UpdateItemDTO updateItemDTO) : IRequest<ErrorOr<Updated>>;

public class UpdateItemHandler(IItemRepository itemRepository, ICurrencyRepository currencyRepository, IMapper mapper) : IRequestHandler<UpdateItemRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateItemRequest request, CancellationToken cancellationToken)
    {
        if (!await itemRepository.ExistsAsync(request.updateItemDTO.Id, cancellationToken) ||
            !await currencyRepository.ExistsAsync(request.updateItemDTO.CurrencyId, cancellationToken))
        {
            return Error.NotFound();
        }

        if ((await itemRepository.GetByIdAsync(request.updateItemDTO.Id, cancellationToken)).ClientId != request.updateItemDTO.ClientId)
        {
            return Error.Conflict();
        }

        var item = mapper.Map<Data.Models.Item>(request.updateItemDTO);

        await itemRepository.UpdateAsync(item, cancellationToken);
        return Result.Updated;
    }
}
