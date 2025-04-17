using FluentValidation;

namespace Exadel.ReportHub.Handlers.User.UpdatePassword;

public class UpdateUserPasswordRequestValidator : AbstractValidator<UpdateUserPasswordRequest>
{
    private readonly IValidator<string> _validator;

    public UpdateUserPasswordRequestValidator(IValidator<string> validator)
    {
        _validator = validator;
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.Password)
            .SetValidator(_validator, Constants.Validation.RuleSet.Passwords);
    }
}
