﻿using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.Csv.Abstract;
using Exadel.ReportHub.Handlers.Validators;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Import;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using FluentValidation;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.Import;

public record ImportInvoicesRequest(ImportDTO ImportDTO) : IRequest<ErrorOr<ImportResultDTO>>;

public class ImportInvoicesHandler(
    ICsvProcessor csvProcessor,
    IInvoiceRepository invoiceRepository,
    IMapper mapper,
    IValidator<CreateInvoiceDTO> invoiceValidator) : IRequestHandler<ImportInvoicesRequest, ErrorOr<ImportResultDTO>>
{
    public async Task<ErrorOr<ImportResultDTO>> Handle(ImportInvoicesRequest request, CancellationToken cancellationToken)
    {
        using var stream = request.ImportDTO.File.OpenReadStream();

        var invoiceDtos = csvProcessor.ReadInvoices(stream);
        var tasks = invoiceDtos.Select(dto => invoiceValidator.ValidateAsync(dto, cancellationToken));
        var validationResults = await Task.WhenAll(tasks);

        var validationErrors = validationResults
            .SelectMany((dto, index) => dto.Errors.Select(m => (RowIndex: index, Error: m.ErrorMessage)))
            .OrderBy(x => x.RowIndex)
            .ToList();

        if(validationErrors.Count > 0)
        {
            return validationErrors
                .Select(m => Error.Validation(description: $"Row {m.RowIndex + 1}: {m.Error}"))
                .ToList();
        }

        var invoices = mapper.Map<IList<Data.Models.Invoice>>(invoiceDtos);
        await invoiceRepository.AddManyAsync(invoices, cancellationToken);

        return new ImportResultDTO { ImportedCount = invoices.Count() };
    }
}
