using FluentValidation;

namespace Exadel.ReportHub.Handlers.Validators;

public class CustomerNameValidator : AbstractValidator<string>
{
    public CustomerNameValidator()
    {
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x)
            .NotEmpty()
            .MaximumLength(Constants.Validation.Customer.NameMaxLength)
            .Matches("^[A-Z]")
            .WithMessage(Constants.Validation.Customer.NameShouldStartWithCapitalMessage);
    }
}
