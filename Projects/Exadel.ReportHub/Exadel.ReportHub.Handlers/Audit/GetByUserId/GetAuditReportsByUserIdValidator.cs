using FluentValidation;

namespace Exadel.ReportHub.Handlers.Audit.GetByUserId;

public class GetAuditReportsByUserIdValidator : AbstractValidator<GetAuditReportsByUserIdRequest>
{
    public GetAuditReportsByUserIdValidator()
    {
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.PageRequestDto)
            .ChildRules(child =>
            {
                child.ClassLevelCascadeMode = CascadeMode.Stop;

                child.RuleFor(x => x.Top)
                    .GreaterThanOrEqualTo(0)
                    .LessThan(Constants.Validation.Pagination.DefaultMaxValue);

                child.RuleFor(x => x.Skip)
                    .GreaterThanOrEqualTo(0);
            });
    }
}
