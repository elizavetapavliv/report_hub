using Exadel.ReportHub.RA.Abstract;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Client.Create;

public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
{
    private readonly IClientRepository _clientRepository;

    public CreateClientRequestValidator(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
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
                    .Matches("^[A-Z]")
                    .WithMessage(Constants.Validation.Client.ShouldStartWithCapitalMessage)
                    .MaximumLength(Constants.Validation.Client.ClientMaximumNameLength)
                    .MustAsync(NameMustNotExistsAsync)
                    .WithMessage(Constants.Validation.Client.NameTakenMessage);
            });
    }

    private async Task<bool> NameMustNotExistsAsync(string name, CancellationToken cancellationToken)
    {
        var nameExists = await _clientRepository.NameExistsAsync(name, cancellationToken);
        return !nameExists;
    }
}
