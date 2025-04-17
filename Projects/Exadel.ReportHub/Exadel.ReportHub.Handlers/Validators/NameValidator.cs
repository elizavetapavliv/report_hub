using FluentValidation;

namespace Exadel.ReportHub.Handlers.Validators;

public class NameValidator : AbstractValidator<string>
{
    public NameValidator()
    {
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x)
            .NotEmpty()
            .MaximumLength(Constants.Validation.Name.NameMaxLength)
            .Matches("^[A-Z]")
            .WithMessage(Constants.Validation.Name.NameShouldStartWithCapitalMessage)
            .WithName(nameof(Constants.Validation.Name));
    }
}
