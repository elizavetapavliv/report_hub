using Exadel.ReportHub.Handlers.Validators;
using Exadel.ReportHub.RA.Abstract;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Item.Update;

public class UpdateItemRequestValidator : AbstractValidator<UpdateItemRequest>
{
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IClientRepository _clientRepository;
    private readonly NameValidator _nameValidator;

    public UpdateItemRequestValidator(ICurrencyRepository currencyRepository, IClientRepository clientRepository, NameValidator nameValidator)
    {
        _currencyRepository = currencyRepository;
        _clientRepository = clientRepository;
        _nameValidator = nameValidator;
        ConfigureRules();
    }

    public void ConfigureRules()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.UpdateItemDTO)
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
                    .SetValidator(_nameValidator);

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
