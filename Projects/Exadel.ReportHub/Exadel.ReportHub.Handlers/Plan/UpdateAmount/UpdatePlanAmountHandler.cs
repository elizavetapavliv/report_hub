using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.Plan.UpdateAmount;

public record UpdatePlanAmountRequest(Guid Id, int Amount) : IRequest<ErrorOr<Updated>>;

public class UpdatePlanAmountHandler(IPlanRepository planRepository) : IRequestHandler<UpdatePlanAmountRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdatePlanAmountRequest request, CancellationToken cancellationToken)
    {
        var isExists = await planRepository.ExistsAsync(request.Id, cancellationToken);
        if (!isExists)
        {
            return Error.NotFound();
        }

        await planRepository.UpdateAmountAsync(request.Id, request.Amount, cancellationToken);
        return Result.Updated;
    }
}
