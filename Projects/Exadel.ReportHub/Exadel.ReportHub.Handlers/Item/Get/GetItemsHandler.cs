using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Item;
using MediatR;

namespace Exadel.ReportHub.Handlers.Item.Get;

public record GetItemsRequest : IRequest<ErrorOr<IEnumerable<ItemDTO>>>;
public class GetItemsHandler(IItemRepository itemRepository, IMapper mapper) : IRequestHandler<GetItemsRequest, ErrorOr<IEnumerable<ItemDTO>>>
{
    public async Task<ErrorOr<IEnumerable<ItemDTO>>> Handle(GetItemsRequest request, CancellationToken cancellationToken)
    {
        return mapper.Map<List<ItemDTO>>(await itemRepository.GetAllAsync(cancellationToken));
    }
}
