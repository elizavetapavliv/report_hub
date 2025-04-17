using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.Item.UpdatePrice;

public record UpdateItemPriceRequest(Guid Id, decimal Price) : IRequest<ErrorOr<Updated>>;

public class UpdateItemPriceHandler(IItemRepository itemRepository) : IRequestHandler<UpdateItemPriceRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateItemPriceRequest request, CancellationToken cancellationToken)
    {
        if (!await itemRepository.ExistsAsync(request.Id, cancellationToken))
        {
            return Error.NotFound();
        }

        await itemRepository.UpdatePriceAsync(request.Id, request.Price, cancellationToken);
        return Result.Updated;
    }
}
