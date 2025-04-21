using Exadel.ReportHub.RA.Abstract;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Invoice.Update;

public class UpdateInvoiceValidator : AbstractValidator<UpdateInvoiceRequest>
{
    public UpdateInvoiceValidator()
    {
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.UpdateInvoiceDto)
          .ChildRules(child =>
          {
              child.RuleLevelCascadeMode = CascadeMode.Stop;

              child.RuleFor(x => x.Amount)
                  .GreaterThan(0);

              child.RuleFor(x => x.IssueDate)
                  .NotEmpty()
                  .LessThan(DateTime.UtcNow)
                  .WithMessage(Constants.Validation.Invoice.IssueDateErrorMessage);

              child.RuleFor(x => x.IssueDate)
                  .Must(date => date.TimeOfDay == TimeSpan.Zero)
                  .WithMessage(Constants.Validation.Invoice.TimeComponentErrorMassage);

              child.RuleFor(x => x.DueDate)
                  .NotEmpty()
                  .GreaterThan(x => x.IssueDate)
                  .WithMessage(Constants.Validation.Invoice.DueDateErrorMessage);

              child.RuleFor(x => x.DueDate)
                  .Must(date => date.TimeOfDay == TimeSpan.Zero)
                  .WithMessage(Constants.Validation.Invoice.TimeComponentErrorMassage);

              child.RuleFor(x => x.PaymentStatus)
                  .IsInEnum();

              child.RuleFor(x => x.BankAccountNumber)
                  .NotEmpty()
                  .Length(Constants.Validation.Invoice.BankAccountNumberMinLength, Constants.Validation.Invoice.BankAccountNumberMaxLength)
                  .Matches(@"^[A-Z]{2}\d+$")
                  .WithMessage(Constants.Validation.Invoice.BankAccountNumberErrorMessage);
          });
    }
}
