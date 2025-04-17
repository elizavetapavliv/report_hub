using Exadel.ReportHub.RA.Abstract;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Plan.Create;

public class CreatePlanRequestValidator : AbstractValidator<CreatePlanRequest>
{
    private readonly IClientRepository _clientRepository;
    private readonly IItemRepository _itemRepository;

    public CreatePlanRequestValidator(IClientRepository clientRepository, IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
        _clientRepository = clientRepository;
        ConfigureRules();
    }

    public void ConfigureRules()
    {
        RuleFor(x => x.CreatePlanDto)
            .ChildRules(child =>
            {
                child.RuleLevelCascadeMode = CascadeMode.Stop;

                child.RuleFor(x => x.ItemId)
                    .NotEmpty()
                    .MustAsync(_itemRepository.ExistsAsync)
                    .WithMessage(Constants.Validation.Plan.ItemDoesNotExistMessage);

                child.RuleFor(x => x.ClientId)
                    .NotEmpty()
                    .MustAsync(_clientRepository.ExistsAsync)
                    .WithMessage(Constants.Validation.Plan.ClientDoesNotExistMessage);

                child.RuleFor(x => x.Amount)
                    .GreaterThan(0);

                child.RuleFor(x => x.StartDate)
                    .NotEmpty()
                    .LessThan(x => x.EndDate)
                    .WithMessage(Constants.Validation.Plan.PlanStartDateErrorMessage);

                child.RuleFor(x => x.EndDate)
                    .NotEmpty()
                    .GreaterThan(DateTime.UtcNow)
                    .WithMessage(Constants.Validation.Plan.PlandEndDateInThePastErrorMessage)
                    .GreaterThan(x => x.StartDate)
                    .WithMessage(Constants.Validation.Plan.PlanEndDateErrorMessage);
            });
    }
}
