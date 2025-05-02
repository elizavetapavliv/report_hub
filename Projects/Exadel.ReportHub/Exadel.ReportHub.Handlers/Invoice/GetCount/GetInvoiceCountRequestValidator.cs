using Exadel.ReportHub.SDK.DTOs.Invoice;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Invoice.GetCount;

public class GetInvoiceCountRequestValidator : AbstractValidator<GetInvoiceCountRequest>
{
    private readonly IValidator<InvoiceFilterDTO> _invoiceFilterDto;

    public GetInvoiceCountRequestValidator(IValidator<InvoiceFilterDTO> invoiceFilterDto)
    {
        _invoiceFilterDto = invoiceFilterDto;
        ConfigureRules();
    }

    public void ConfigureRules()
    {
        RuleFor(x => x.InvoiceFilterDto)
            .SetValidator(_invoiceFilterDto);
    }
}
