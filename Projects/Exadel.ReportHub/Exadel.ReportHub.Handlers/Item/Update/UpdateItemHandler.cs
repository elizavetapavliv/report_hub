using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Item;
using MediatR;

namespace Exadel.ReportHub.Handlers.Item.Update;

public record UpdateItemRequest(Guid Id, CreateUpdateItemDTO UpdateItemDTO) : IRequest<ErrorOr<Updated>>;

public class UpdateItemHandler(IItemRepository itemRepository, ICurrencyRepository currencyRepository, IMapper mapper) : IRequestHandler<UpdateItemRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateItemRequest request, CancellationToken cancellationToken)
    {
        if (!await itemRepository.ExistsAsync(request.Id, cancellationToken))
        {
            return Error.NotFound();
        }

        if ((await itemRepository.GetByIdAsync(request.Id, cancellationToken)).ClientId != request.UpdateItemDTO.ClientId)
        {
            return Error.Conflict();
        }

        var item = mapper.Map<Data.Models.Item>(request.UpdateItemDTO);
        item.Id = request.Id;
        item.CurrencyCode = (await currencyRepository.GetByIdAsync(request.UpdateItemDTO.CurrencyId, cancellationToken)).CurrencyCode;

        await itemRepository.UpdateAsync(item, cancellationToken);
        return Result.Updated;
    }
}
