using Exadel.ReportHub.SDK.DTOs.Invoice;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Invoice.GetCount;

public class GetInvoiceCountRequestValidator : AbstractValidator<GetInvoiceCountRequest>
{
    private readonly IValidator<DatesDTO> _datesDto;

    public GetInvoiceCountRequestValidator(IValidator<DatesDTO> datesDto)
    {
        _datesDto = datesDto;
        ConfigureRules();
    }

    public void ConfigureRules()
    {
        RuleFor(x => x.InvoiceFilterDto)
            .SetValidator(_datesDto);
    }
}
