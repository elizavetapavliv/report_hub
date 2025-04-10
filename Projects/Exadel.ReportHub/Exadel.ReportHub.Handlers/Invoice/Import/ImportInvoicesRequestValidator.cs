using System;
using System.Collections.Generic;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Invoice.Import;

public class ImportInvoicesRequestValidator : AbstractValidator<ImportInvoicesRequest>
{
    public ImportInvoicesRequestValidator()
    {
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.CsvStream)
                .NotNull()
                .Must(stream => stream.CanRead);

        RuleFor(x => x.FileName)
            .NotEmpty()
            .Must(fileName => string.Equals(Path.GetExtension(fileName), ".csv", StringComparison.OrdinalIgnoreCase))
            .WithMessage("The file must be in CSV format (extension .csv).");
    }
}
