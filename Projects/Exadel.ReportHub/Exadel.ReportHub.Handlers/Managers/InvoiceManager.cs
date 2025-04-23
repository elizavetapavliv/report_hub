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
    public async Task<IList<Data.Models.Invoice>> GenerateInvoicesAsync(IEnumerable<CreateInvoiceDTO> invoiceDtos, CancellationToken cancellationToken)
    {
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

        return invoices;
    }
}
