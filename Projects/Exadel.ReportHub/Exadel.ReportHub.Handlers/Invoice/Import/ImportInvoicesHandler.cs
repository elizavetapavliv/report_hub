using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.Csv.Abstract;
using Exadel.ReportHub.Ecb.Abstract;
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
    ICustomerRepository customerRepository,
    IItemRepository itemRepository,
    ICurrencyConverter currencyConverter,
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

        if (validationErrors.Count > 0)
        {
            return validationErrors
                .Select(m => Error.Validation(description: $"Row {m.RowIndex + 1}: {m.Error}"))
                .ToList();
        }

        var invoices = mapper.Map<IList<Data.Models.Invoice>>(invoiceDtos);

        var customerIds = invoices.Select(x => x.CustomerId).Distinct().ToList();
        var itemIds = invoices.SelectMany(x => x.ItemIds).Distinct().ToList();

        var customersTask = customerRepository.GetByIdsAsync(customerIds, cancellationToken);
        var itemsTask = itemRepository.GetByIdsAsync(itemIds, cancellationToken);

        await Task.WhenAll(customersTask, itemsTask);

        var customers = customersTask.Result.ToDictionary(x => x.Id);
        var items = itemsTask.Result.ToDictionary(x => x.Id);

        foreach (var invoice in invoices)
        {
            var conversionTasks = invoice.ItemIds.Select(itemId => items[itemId]).GroupBy(x => x.CurrencyCode)
                .Select(group => currencyConverter.ConvertAsync(group.Sum(x => x.Price), group.Key, customers[invoice.CustomerId].CurrencyCode, cancellationToken));

            invoice.Id = Guid.NewGuid();
            invoice.Amount = (await Task.WhenAll(conversionTasks)).Sum();
            invoice.CurrencyId = customers[invoice.CustomerId].CurrencyId;
            invoice.Currency = customers[invoice.CustomerId].CurrencyCode;
        }

        await invoiceRepository.AddManyAsync(invoices, cancellationToken);

        return new ImportResultDTO { ImportedCount = invoices.Count };
    }
}
