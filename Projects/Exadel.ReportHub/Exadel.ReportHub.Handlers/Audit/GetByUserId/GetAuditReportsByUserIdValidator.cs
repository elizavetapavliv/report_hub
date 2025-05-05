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
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage(Constants.Validation.Pagination.InvalidValue);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage(Constants.Validation.Pagination.InvalidValue)
            .LessThanOrEqualTo(Constants.Validation.Pagination.MaxPageSize)
            .WithMessage(Constants.Validation.Pagination.InvalidSize);
    }
}
