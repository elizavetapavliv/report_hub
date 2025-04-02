using System.Security;
using Exadel.ReportHub.Handlers.Validators;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.User.UpdatePassword;

public class UpdateUserPasswordRequestValidator : AbstractValidator<UpdateUserPasswordRequest>
{
    private readonly PasswordValidator _passwordValidator;

    public UpdateUserPasswordRequestValidator(PasswordValidator passwordValidator)
    {
        _passwordValidator = passwordValidator;
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.Password)
            .SetValidator(_passwordValidator);
    }
}
