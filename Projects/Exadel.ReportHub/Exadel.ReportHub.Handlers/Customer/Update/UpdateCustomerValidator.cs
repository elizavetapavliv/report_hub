using FluentValidation;

namespace Exadel.ReportHub.Handlers.Customer.Update;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerRequest>
{
    private readonly IValidator<string> _validator;

    public UpdateCustomerValidator(IValidator<string> validator)
    {
        _validator = validator;
        ConfigureRules();
    }

    public void ConfigureRules()
    {
        RuleFor(x => x.UpdateCustomerDto)
           .ChildRules(child =>
           {
               child.RuleLevelCascadeMode = CascadeMode.Stop;

               child.RuleFor(x => x.Name)
                   .SetValidator(_validator, Constants.Validation.RuleSet.Names);

               child.RuleFor(x => x.Country)
                   .SetValidator(_validator, Constants.Validation.RuleSet.Countries);
           });
    }
}
