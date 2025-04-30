using Exadel.ReportHub.SDK.DTOs.Invoice;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Validators;

public class InvoiceDateFilterValidator : AbstractValidator<InvoiceIssueDateFilterDTO>
{
    public InvoiceDateFilterValidator()
    {
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x)
            .ChildRules(child =>
            {
                RuleLevelCascadeMode = CascadeMode.Stop;

                child.RuleFor(x => x.StartDate)
                    .NotEmpty()
                    .WithMessage("Start date is required")
                    .LessThanOrEqualTo(x => x.EndDate)
                    .WithMessage("Start date must be less than or equal to end date");

                child.RuleFor(x => x.StartDate.TimeOfDay)
                    .Equal(TimeSpan.Zero)
                    .WithMessage(Constants.Validation.Invoice.TimeComponentNotAllowed);
                child.RuleFor(x => x.EndDate)
                    .NotEmpty();

                child.RuleFor(x => x.EndDate.TimeOfDay)
                    .Equal(TimeSpan.Zero)
                    .WithMessage(Constants.Validation.Invoice.TimeComponentNotAllowed);
            });
    }
}
