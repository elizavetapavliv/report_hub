using Exadel.ReportHub.Handlers.Validators;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Customer.UpdateName;

public class UpdateCustomerNameRequestValidator : AbstractValidator<UpdateCustomerNameRequest>
{
    public UpdateCustomerNameRequestValidator()
    {
        ConfigureRules();
    }

    public void ConfigureRules()
    {
        RuleFor(x => x.Name)
            .SetValidator(new CustomerNameValidator());
    }
}
