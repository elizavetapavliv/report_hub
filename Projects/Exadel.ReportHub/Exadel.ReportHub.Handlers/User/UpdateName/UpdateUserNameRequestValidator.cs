using FluentValidation;

namespace Exadel.ReportHub.Handlers.User.UpdateName;

public class UpdateUserNameRequestValidator : AbstractValidator<UpdateUserNameRequest>
{
    public UpdateUserNameRequestValidator()
    {
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(Constants.Validation.User.FullNameMaxLength);
    }
}
