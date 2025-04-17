using Exadel.ReportHub.Handlers.Validators;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Client.Create;

public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
{
    private readonly ClientNameValidator _clientNameValidator;

    public CreateClientRequestValidator(ClientNameValidator clientNameValidator)
    {
        _clientNameValidator = clientNameValidator;
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.CreateClientDto)
            .NotEmpty()
            .ChildRules(child =>
            {
                child.RuleFor(x => x.Name)
                .NotEmpty()
                .SetValidator(_clientNameValidator);
            });
    }
}
