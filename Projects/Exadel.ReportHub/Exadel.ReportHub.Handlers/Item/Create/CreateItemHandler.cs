using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Item;
using MediatR;

namespace Exadel.ReportHub.Handlers.Item.Create;

public record CreateItemRequest(CreateItemDTO createItemDto) : IRequest<ErrorOr<ItemDTO>>;
public class CreateItemHandler(IItemRepository itemRepository, IMapper mapper) : IRequestHandler<CreateItemRequest, ErrorOr<ItemDTO>>
{
    public async Task<ErrorOr<ItemDTO>> Handle(CreateItemRequest request, CancellationToken cancellationToken)
    {
        var item = mapper.Map<Data.Models.Item>(request.createItemDto);
        item.Id = Guid.NewGuid();

        await itemRepository.AddAsync(item, cancellationToken);

        return mapper.Map<ItemDTO>(item);
    }
}
