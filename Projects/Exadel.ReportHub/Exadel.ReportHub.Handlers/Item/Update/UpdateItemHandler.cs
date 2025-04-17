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
        var itemClientId = await itemRepository.GetClientIdAsync(request.Id, cancellationToken);
        if (itemClientId == Guid.Empty)
        {
            return Error.NotFound();
        }

        if (itemClientId != request.UpdateItemDTO.ClientId)
        {
            return Error.Validation(description: "Client Id cannot be changed.");
        }

        var item = mapper.Map<Data.Models.Item>(request.UpdateItemDTO);
        item.Id = request.Id;
        item.CurrencyCode = (await currencyRepository.GetByIdAsync(request.UpdateItemDTO.CurrencyId, cancellationToken)).CurrencyCode;

        await itemRepository.UpdateAsync(item, cancellationToken);
        return Result.Updated;
    }
}
