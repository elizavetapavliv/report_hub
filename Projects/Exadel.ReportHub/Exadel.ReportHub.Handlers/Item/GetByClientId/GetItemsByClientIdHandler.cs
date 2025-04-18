using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Item;
using MediatR;

namespace Exadel.ReportHub.Handlers.Item.GetByClientId;

public record GetItemsByClientIdRequest(Guid ClientId) : IRequest<ErrorOr<IEnumerable<ItemDTO>>>;

public class GetItemsByClientIdHandler(IItemRepository itemRepository, IMapper mapper) : IRequestHandler<GetItemsByClientIdRequest, ErrorOr<IEnumerable<ItemDTO>>>
{
    public async Task<ErrorOr<IEnumerable<ItemDTO>>> Handle(GetItemsByClientIdRequest request, CancellationToken cancellationToken)
    {
        return mapper.Map<List<ItemDTO>>(await itemRepository.GetByClientIdAsync(request.ClientId, cancellationToken));
    }
}
