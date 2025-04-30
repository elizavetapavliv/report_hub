using Exadel.ReportHub.Handlers.Invoice.GetTotalNumber;
using Exadel.ReportHub.Handlers.Validators;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Invoice.GetNumber;

public class GetInvoicesNumberRequestValidator : AbstractValidator<GetInvoicesNumberRequest>
{
    private readonly IValidator<InvoiceIssueDateFilterDTO> _invoiceDateFilterValidator;

    public GetInvoicesNumberRequestValidator(IValidator<InvoiceIssueDateFilterDTO> invoiceDateFilterValidator)
    {
        _invoiceDateFilterValidator = invoiceDateFilterValidator;
        ConfigureRules();
    }

    public void ConfigureRules()
    {
        RuleFor(x => x.InvoiceIssueDateFilterDto)
            .SetValidator(_invoiceDateFilterValidator);
    }
}
