using Exadel.ReportHub.SDK.DTOs.Date;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Validators;

public class FilterDataValidator : AbstractValidator<DatesDTO>
{
    public FilterDataValidator()
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
                    .WithMessage(Constants.Validation.Date.EmptyStartDate)
                    .LessThanOrEqualTo(x => x.EndDate)
                    .WithMessage(Constants.Validation.Date.InvalidStartDate);

                child.RuleFor(x => x.EndDate)
                    .NotEmpty()
                    .GreaterThan(DateTime.UtcNow)
                    .WithMessage(Constants.Validation.Date.EndDateInPast);

                child.RuleFor(x => x.StartDate.TimeOfDay)
                    .Equal(TimeSpan.Zero)
                    .WithMessage(Constants.Validation.Invoice.TimeComponentNotAllowed);

                child.RuleFor(x => x.EndDate.TimeOfDay)
                    .Equal(TimeSpan.Zero)
                    .WithMessage(Constants.Validation.Invoice.TimeComponentNotAllowed);
            });
    }
}
