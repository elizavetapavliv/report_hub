using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Plan;
using MediatR;

namespace Exadel.ReportHub.Handlers.Plan.UpdateDate;

public record UpdatePlanDateRequest(Guid Id, UpdatePlanDateDTO UpdatePlanDatedto) : IRequest<ErrorOr<Updated>>;

public class UpdatePlanDateHandler(IPlanRepository planRepository, IMapper mapper) : IRequestHandler<UpdatePlanDateRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdatePlanDateRequest request, CancellationToken cancellationToken)
    {
        var isExists = await planRepository.ExistsAsync(request.Id, cancellationToken);
        if (!isExists)
        {
            return Error.NotFound();
        }

        var plan = mapper.Map<Data.Models.Plan>(request.UpdatePlanDatedto);
        await planRepository.UpdateDateAsync(request.Id, plan, cancellationToken);
        return Result.Updated;
    }
}
