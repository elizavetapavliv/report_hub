using Exadel.ReportHub.SDK.DTOs.Invoice;
using Exadel.ReportHub.SDK.Enums;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Validators;

public class InvoiceDtoValidator : AbstractValidator<CreateInvoiceDTO>
{
    private const int CurrencyCodeLength = 3;
    private const int InvoiceMaximumNumberLength = 15;
    private const string InvoiceNumberErrorMessage = "Invoice number must start with 'INV' followed by digits.";
    private const string IssueDateErrorMessage = "Issue date cannot be in the future.";
    private const string DueDateErrorMessage = "Due date must be greater than or equal to issue date.";
    private const string PaymentStatusErrorMessage = "Invalid payment status.";
    private const string ItemIdsErrorMessage = "At least one item must be selected.";

    public InvoiceDtoValidator()
    {
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.ClientId)
            .NotEmpty();

        RuleFor(x => x.CustomerId)
            .NotEmpty();

        RuleFor(x => x.InvoiceNumber)
            .NotEmpty()
            .MaximumLength(InvoiceMaximumNumberLength)
            .Matches(@"^INV\d+$")
            .WithMessage(InvoiceNumberErrorMessage);

        RuleFor(x => x.IssueDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(IssueDateErrorMessage);

        RuleFor(x => x.DueDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.IssueDate)
            .WithMessage(DueDateErrorMessage);

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Length(CurrencyCodeLength)
            .WithMessage($"Currency must be exactly {CurrencyCodeLength} characters long.")
            .Matches(@"^[A-Z]+$")
            .WithMessage($"Currency code must be exactly {CurrencyCodeLength} uppercase letters.");

        RuleFor(x => x.PaymentStatus)
            .Must(BeValidPaymentStatus)
            .WithMessage(PaymentStatusErrorMessage);

        RuleFor(x => x.BankAccountNumber)
            .NotEmpty();

        RuleFor(x => x.ItemIds)
            .Must(x => x.Count > 0).WithMessage(ItemIdsErrorMessage);
    }

    private bool BeValidPaymentStatus(PaymentStatus paymentStatus)
    {
        return Enum.IsDefined(typeof(PaymentStatus), paymentStatus);
    }
}
