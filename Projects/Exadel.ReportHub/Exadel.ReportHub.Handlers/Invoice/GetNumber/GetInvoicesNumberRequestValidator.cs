using Exadel.ReportHub.Handlers.Invoice.GetNumber;
using Exadel.ReportHub.Handlers.Validators;
using Exadel.ReportHub.SDK.DTOs.Date;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Invoice.GetNumber;

public class GetInvoicesNumberRequestValidator : AbstractValidator<GetInvoicesNumberRequest>
{
    private readonly IValidator<DatesDTO> _datesDto;

    public GetInvoicesNumberRequestValidator(IValidator<DatesDTO> datesDto)
    {
        _datesDto = datesDto;
        ConfigureRules();
    }

    public void ConfigureRules()
    {
        RuleFor(x => x.InvoiceFilterDTO)
            .SetValidator(_datesDto);
    }
}
