using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.Csv.Abstract;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using Exadel.ReportHub.SDK.Models;
using FluentValidation;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.Import;

public record ImportInvoicesRequest(FileModel Model) : IRequest<ErrorOr<string>>;

public class ImportInvoicesHandler(
    ICsvProcessor csvProcessor,
    IInvoiceRepository invoiceRepository,
    IMapper mapper,
    IValidator<CreateInvoiceDTO> invoiceValidator) : IRequestHandler<ImportInvoicesRequest, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(ImportInvoicesRequest request, CancellationToken cancellationToken)
    {
        using var stream = request.Model.FormFile.OpenReadStream();

        var invoiceDtos = csvProcessor.ReadInvoices(stream);

        var validationErrors = new List<string>();
        var validInvoiceDtos = new List<CreateInvoiceDTO>();
        foreach (var invoice in invoiceDtos)
        {
            var result = await invoiceValidator.ValidateAsync(invoice, cancellationToken);
            if (!result.IsValid)
            {
                validationErrors.AddRange(result.Errors.Select(e => e.ErrorMessage));
                continue;
            }

            validInvoiceDtos.Add(invoice);
        }

        if(validationErrors.Any())
        {
            return Error.Validation("Invoice_Validation_Error", string.Join("; ", validationErrors));
        }

        var invoices = mapper.Map<IEnumerable<Data.Models.Invoice>>(validInvoiceDtos);

        await invoiceRepository.AddManyAsync(invoices, cancellationToken);

        return $"{invoices.Count()} invoices imported";
    }
}
