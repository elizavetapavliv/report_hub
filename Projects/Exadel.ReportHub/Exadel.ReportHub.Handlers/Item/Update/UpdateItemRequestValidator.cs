using Exadel.ReportHub.Handlers.Validators;
using Exadel.ReportHub.RA;
using Exadel.ReportHub.RA.Abstract;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Item.Update;

public class UpdateItemRequestValidator(IClientRepository clientRepository, ICurrencyRepository currencyRepository) : AbstractValidator<UpdateItemRequest>
{
    public void ConfigureRules()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.UpdateItemDTO)
            .ChildRules(child =>
            {
                child.RuleLevelCascadeMode = CascadeMode.Stop;

                child.RuleFor(x => x.ClientId)
                    .NotEmpty()
                    .MustAsync(clientRepository.ExistsAsync)
                    .WithMessage(Constants.Validation.Item.ClientDoesNotExistMessage);

                child.RuleFor(x => x.CurrencyId)
                    .NotEmpty()
                    .MustAsync(currencyRepository.ExistsAsync)
                    .WithMessage(Constants.Validation.Item.CurrencyDoesNotExistMessage);

                child.RuleFor(x => x.Name)
                    .SetValidator(new NameValidator());

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
