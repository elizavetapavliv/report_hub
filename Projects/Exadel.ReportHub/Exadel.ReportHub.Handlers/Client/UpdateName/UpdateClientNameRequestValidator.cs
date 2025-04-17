using Exadel.ReportHub.Handlers.Validators;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Client.UpdateName;

public class UpdateClientNameRequestValidator : AbstractValidator<UpdateClientNameRequest>
{
    private readonly ClientNameValidator _clientNameValidator;

    public UpdateClientNameRequestValidator(ClientNameValidator clientNameValidator)
    {
        _clientNameValidator = clientNameValidator;
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .SetValidator(_clientNameValidator);
    }
}
