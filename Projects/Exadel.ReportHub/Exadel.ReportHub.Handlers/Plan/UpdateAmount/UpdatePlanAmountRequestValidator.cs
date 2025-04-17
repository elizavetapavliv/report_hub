using FluentValidation;

namespace Exadel.ReportHub.Handlers.Plan.UpdateAmount;

public class UpdatePlanAmountRequestValidator : AbstractValidator<UpdatePlanAmountRequest>
{
    public UpdatePlanAmountRequestValidator()
    {
        ConfigureRules();
    }

    public void ConfigureRules()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}
