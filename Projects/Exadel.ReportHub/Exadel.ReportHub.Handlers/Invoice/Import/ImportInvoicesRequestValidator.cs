using Exadel.ReportHub.SDK.DTOs.Import;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Invoice.Import;

public class ImportInvoicesRequestValidator : AbstractValidator<ImportInvoicesRequest>
{
    private readonly IValidator<ImportDTO> _importDtoValidator;

    public ImportInvoicesRequestValidator(IValidator<ImportDTO> importDtoValidator)
    {
        _importDtoValidator = importDtoValidator;
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.ImportDTO)
            .SetValidator(_importDtoValidator);
        RuleFor(x => x.ImportDTO.File.FileName)
            .Must(fileName => string.Equals(Path.GetExtension(fileName), Constants.File.Extension.Csv, StringComparison.OrdinalIgnoreCase))
            .WithMessage(Constants.Validation.Import.InvalidFileExtension);
    }
}
