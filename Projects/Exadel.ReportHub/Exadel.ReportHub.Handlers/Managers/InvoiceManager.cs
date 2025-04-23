using AutoMapper;
using Exadel.ReportHub.Ecb.Abstract;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;

namespace Exadel.ReportHub.Handlers.Managers;

public class InvoiceManager(
    ICustomerRepository customerRepository,
    IItemRepository itemRepository,
    ICurrencyConverter currencyConverter,
    IMapper mapper) : IInvoiceManager
{
    public async Task<Data.Models.Invoice> GenerateInvoiceAsync(CreateInvoiceDTO invoiceDto, CancellationToken cancellationToken)
    {
        var invoice = mapper.Map<Data.Models.Invoice>(invoiceDto);

        var customerTask = customerRepository.GetByIdAsync(invoice.CustomerId, cancellationToken);
        var itemsTask = itemRepository.GetByIdsAsync(invoice.ItemIds, cancellationToken);

        await Task.WhenAll(customerTask, itemsTask);

        var currencyCode = customerTask.Result.CurrencyCode;
        var conversionTasks = itemsTask.Result.GroupBy(x => x.CurrencyCode)
            .Select(group => currencyConverter.ConvertAsync(group.Sum(x => x.Price), group.Key, currencyCode, cancellationToken));

        invoice.Id = Guid.NewGuid();
        invoice.Amount = (await Task.WhenAll(conversionTasks)).Sum();
        invoice.CurrencyId = customerTask.Result.CurrencyId;
        invoice.CurrencyCode = currencyCode;

        return invoice;
    }

    public async Task<IList<Data.Models.Invoice>> GenerateInvoicesAsync(IEnumerable<CreateInvoiceDTO> invoiceDtos, CancellationToken cancellationToken)
    {
        var invoiceTasks = invoiceDtos.Select(dto => GenerateInvoiceAsync(dto, cancellationToken));
        var invoices = await Task.WhenAll(invoiceTasks);

        return invoices;
    }
}
