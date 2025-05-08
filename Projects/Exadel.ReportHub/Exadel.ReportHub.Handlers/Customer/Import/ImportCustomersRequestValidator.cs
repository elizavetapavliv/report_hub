﻿using Exadel.ReportHub.SDK.DTOs.Import;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Customer.Import;

public class ImportCustomersRequestValidator : AbstractValidator<ImportCustomersRequest>
{
    private readonly IValidator<ImportDTO> _importDtoValidator;

    public ImportCustomersRequestValidator(IValidator<ImportDTO> importDtoValidator)
    {
        ConfigureRules();
        _importDtoValidator = importDtoValidator;
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.ImportDTO)
            .SetValidator(_importDtoValidator);
        RuleFor(x => x.ImportDTO.File.FileName)
            .Must(fileName => string.Equals(Path.GetExtension(fileName), ".xlsx", StringComparison.OrdinalIgnoreCase))
            .WithMessage(Constants.Validation.Import.InvalidFileExtension);
    }
}
