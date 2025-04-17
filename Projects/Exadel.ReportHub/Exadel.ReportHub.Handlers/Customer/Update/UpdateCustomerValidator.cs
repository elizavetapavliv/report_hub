using Exadel.ReportHub.Handlers.Validators;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Customer.Update;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerRequest>
{
    private readonly CountryValidator _countryValidator;
    private readonly NameValidator _nameValidator;

    public UpdateCustomerValidator(CountryValidator countryValidator, NameValidator nameValidator)
    {
        _countryValidator = countryValidator;
        _nameValidator = nameValidator;
        ConfigureRules();
    }

    public void ConfigureRules()
    {
        RuleFor(x => x.UpdateCustomerDto)
           .ChildRules(child =>
           {
               child.RuleLevelCascadeMode = CascadeMode.Stop;

               child.RuleFor(x => x.Name)
                   .SetValidator(_nameValidator);

               child.RuleFor(x => x.Country)
                   .SetValidator(_countryValidator);
           });
    }
}
