﻿using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Plan;
using MediatR;

namespace Exadel.ReportHub.Handlers.Plan.GetByClientId;

public record GetPlansByClientIdRequest(Guid ClientId) : IRequest<ErrorOr<IList<PlanDTO>>>;

public class GetPlansByClientIdHandler(IPlanRepository planRepository, IMapper mapper, IClientRepository clientRepository) : IRequestHandler<GetPlansByClientIdRequest, ErrorOr<IList<PlanDTO>>>
{
    public async Task<ErrorOr<IList<PlanDTO>>> Handle(GetPlansByClientIdRequest request, CancellationToken cancellationToken)
    {
        var isExists = await clientRepository.ExistsAsync(request.ClientId, cancellationToken);
        if (!isExists)
        {
            return Error.NotFound();
        }

        var plans = await planRepository.GetByClientIdAsync(request.ClientId, cancellationToken);

        var plansDto = mapper.Map<List<PlanDTO>>(plans);
        return plansDto;
    }
}
