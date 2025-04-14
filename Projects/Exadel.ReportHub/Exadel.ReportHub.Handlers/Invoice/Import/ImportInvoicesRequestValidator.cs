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
        RuleFor(x => x.ImportDTO)
                .NotNull();

        RuleFor(x => x.ImportDTO.FormFile)
                .NotNull()
                .Must(file => file.Length > 0)
                .DependentRules(() =>
                {
                    RuleFor(x => x.ImportDTO.FormFile.FileName)
                        .NotEmpty()
                        .Must(fileName => string.Equals(Path.GetExtension(fileName), ".csv", StringComparison.OrdinalIgnoreCase))
                        .WithMessage("The file must be in CSV format (.csv extension).");
                });
    }
}
