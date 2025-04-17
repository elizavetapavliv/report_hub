using Exadel.ReportHub.RA.Abstract;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Item.Create;

public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
{
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IValidator<string> _validator;

    public CreateItemRequestValidator(ICurrencyRepository currencyRepository, IClientRepository clientRepository, IValidator<string> validator)
    {
        _currencyRepository = currencyRepository;
        _clientRepository = clientRepository;
        _validator = validator;
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.CreateItemDto)
            .ChildRules(child =>
            {
                child.RuleLevelCascadeMode = CascadeMode.Stop;

                child.RuleFor(x => x.ClientId)
                    .NotEmpty()
                    .MustAsync(_clientRepository.ExistsAsync)
                    .WithMessage(Constants.Validation.Item.ClientDoesNotExistMessage);

                child.RuleFor(x => x.CurrencyId)
                    .NotEmpty()
                    .MustAsync(_currencyRepository.ExistsAsync)
                    .WithMessage(Constants.Validation.Item.CurrencyDoesNotExistMessage);

                child.RuleFor(x => x.Name)
                    .SetValidator(_validator, Constants.Validation.RuleSet.Names);

                child.RuleFor(x => x.Description)
                    .NotEmpty()
                    .MaximumLength(Constants.Validation.Item.DescriptionMaxLength)
                    .Matches("^[A-Z]")
                    .WithMessage(Constants.Validation.Item.DescriptionShouldStartWithCapitalMessage);

                child.RuleFor(x => x.Price)
                    .NotEmpty()
                    .GreaterThan(0)
                    .WithMessage(Constants.Validation.Item.NegativePriceErrorMessage);
            });
    }
}
