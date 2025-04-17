using Exadel.ReportHub.RA.Abstract;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Plan.UpdateDate;

public class UpdatePlanDateRequestValidator : AbstractValidator<UpdatePlanDateRequest>
{
    public UpdatePlanDateRequestValidator()
    {
        ConfigureRules();
    }

    public void ConfigureRules()
    {
        RuleFor(x => x.UpdatePlanDatedto)
            .ChildRules(child =>
            {
                child.RuleLevelCascadeMode = CascadeMode.Stop;

                child.RuleFor(x => x.StartDate)
                    .NotEmpty()
                    .LessThan(x => x.EndDate)
                    .WithMessage(Constants.Validation.Plan.PlanStartDateErrorMessage);

                child.RuleFor(x => x.EndDate)
                    .NotEmpty()
                    .GreaterThan(DateTime.UtcNow)
                    .WithMessage(Constants.Validation.Plan.PlandEndDateInThePastErrorMessage)
                    .GreaterThan(x => x.StartDate)
                    .WithMessage(Constants.Validation.Plan.PlanEndDateErrorMessage);
            });
    }
}
