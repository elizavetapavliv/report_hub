using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Plan;
using MediatR;

namespace Exadel.ReportHub.Handlers.Plan.Get;

public record GetPlansRequest : IRequest<ErrorOr<IEnumerable<PlanDTO>>>;

public class GetPlansHandler(IPlanRepository planRepository, IMapper mapper) : IRequestHandler<GetPlansRequest, ErrorOr<IEnumerable<PlanDTO>>>
{
    public async Task<ErrorOr<IEnumerable<PlanDTO>>> Handle(GetPlansRequest request, CancellationToken cancellationToken)
    {
        var plans = await planRepository.GetAsync(cancellationToken);

        var plansDto = mapper.Map<List<PlanDTO>>(plans);
        return plansDto;
    }
}
