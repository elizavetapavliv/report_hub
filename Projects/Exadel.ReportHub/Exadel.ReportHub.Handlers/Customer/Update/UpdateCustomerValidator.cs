using Exadel.ReportHub.RA.Abstract;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Customer.Update;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerRequest>
{
    private readonly ICountryRepository _countryRepository;
    private readonly IValidator<string> _stringValidator;

    public UpdateCustomerValidator(ICountryRepository countryRepository, IValidator<string> stringValidator)
    {
        _countryRepository = countryRepository;
        _stringValidator = stringValidator;
        ConfigureRules();
    }

    public void ConfigureRules()
    {
        RuleFor(x => x.UpdateCustomerDto)
           .ChildRules(child =>
           {
               child.RuleLevelCascadeMode = CascadeMode.Stop;

               child.RuleFor(x => x.Name)
                   .SetValidator(_stringValidator, Constants.Validation.RuleSet.Names);

               child.RuleFor(x => x.CountryId)
                   .NotEmpty()
                   .MustAsync(_countryRepository.ExistsAsync)
                   .WithMessage(Constants.Validation.Customer.CountryDoesNotExistMessage);
           });
    }
}
