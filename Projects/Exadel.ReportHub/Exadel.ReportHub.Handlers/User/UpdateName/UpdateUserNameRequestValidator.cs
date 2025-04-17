using FluentValidation;

namespace Exadel.ReportHub.Handlers.User.UpdateName;

public class UpdateUserNameRequestValidator : AbstractValidator<UpdateUserNameRequest>
{
    private readonly IValidator<string> _validator;

    public UpdateUserNameRequestValidator(IValidator<string> validator)
    {
        _validator = validator;
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.FullName)
            .SetValidator(_validator, Constants.Validation.RuleSet.Names);
    }
}
