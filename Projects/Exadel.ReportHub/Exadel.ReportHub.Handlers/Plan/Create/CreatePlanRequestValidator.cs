﻿using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Plan;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Plan.Create;

public class CreatePlanRequestValidator : AbstractValidator<CreatePlanRequest>
{
    private readonly IClientRepository _clientRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IPlanRepository _planRepository;

    public CreatePlanRequestValidator(IClientRepository clientRepository, IItemRepository itemRepository, IPlanRepository planRepository)
    {
        _planRepository = planRepository;
        _itemRepository = itemRepository;
        _clientRepository = clientRepository;
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.CreatePlanDto)
            .ChildRules(child =>
            {
                child.RuleLevelCascadeMode = CascadeMode.Stop;

                child.RuleFor(x => x)
                    .MustAsync(IsUniquePlanAsync)
                    .WithMessage(Constants.Validation.Plan.AlreadyExistsForItemAndClient);

                child.RuleFor(x => x.ItemId)
                    .NotEmpty()
                    .MustAsync(_itemRepository.ExistsAsync)
                    .WithMessage(Constants.Validation.Item.DoesNotExist);

                child.RuleFor(x => x.ClientId)
                    .NotEmpty()
                    .MustAsync(_clientRepository.ExistsAsync)
                    .WithMessage(Constants.Validation.Client.DoesNotExist);

                child.RuleFor(x => x.Count)
                    .GreaterThan(0);

                child.RuleFor(x => x.StartDate)
                    .NotEmpty()
                    .LessThan(x => x.EndDate)
                    .WithMessage(Constants.Validation.Date.InvalidStartDate);

                child.RuleFor(x => x.EndDate)
                    .NotEmpty()
                    .GreaterThan(DateTime.UtcNow)
                    .WithMessage(Constants.Validation.Date.EndDateInPast);
            });
    }

    private async Task<bool> IsUniquePlanAsync(CreatePlanDTO createPlanDto, CancellationToken cancellationToken)
    {
        var isExists = await _planRepository.ExistsForItemByPeriodAsync(
            createPlanDto.ItemId, createPlanDto.ClientId, createPlanDto.StartDate, createPlanDto.EndDate, cancellationToken);
        return !isExists;
    }
}
