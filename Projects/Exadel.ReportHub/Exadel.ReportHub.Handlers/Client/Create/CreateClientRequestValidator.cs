using Exadel.ReportHub.RA.Abstract;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Client.Create;

public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
{
    private readonly IClientRepository _clientRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IValidator<string> _stringValidator;

    public CreateClientRequestValidator(ICountryRepository countryRepository, IClientRepository clientRepository, IValidator<string> stringValidator)
    {
        _countryRepository = countryRepository;
        _clientRepository = clientRepository;
        _stringValidator = stringValidator;
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.CreateClientDto)
            .NotEmpty()
            .ChildRules(child =>
            {
                child.RuleLevelCascadeMode = CascadeMode.Stop;

                child.RuleFor(x => x.Name)
                    .SetValidator(_stringValidator, Constants.Validation.RuleSet.Names)
                    .MustAsync(async (name, cancellationToken) => !await _clientRepository.NameExistsAsync(name, cancellationToken))
                    .WithMessage(Constants.Validation.Name.AlreadyTaken);
                child.RuleFor(x => x.BankAccountNumber)
                    .NotEmpty()
                    .Length(Constants.Validation.BankAccountNumber.MinLength, Constants.Validation.BankAccountNumber.MaxLength)
                    .Matches(@"^[A-Z]{2}\d+$")
                    .WithMessage(Constants.Validation.BankAccountNumber.InvalidFormat)
                    .MustAsync((x, bankAccountNumber, cancellationToken)
                    => ValidateBankAccountNumberAsync(x.CountryId, bankAccountNumber, cancellationToken))
                    .WithMessage(Constants.Validation.BankAccountNumber.InvalidCountryCode);
            });
    }

    private async Task<bool> ValidateBankAccountNumberAsync(Guid countryId, string bankAccountNumber, CancellationToken cancellationToken)
    {
        var country = await _countryRepository.GetByIdAsync(countryId, cancellationToken);
        if(country == null)
        {
            return false;
        }

        var countryCode = bankAccountNumber.Substring(0, 2);
        return countryCode == country.CountryCode;
    }
}
