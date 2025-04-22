using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.Ecb.Abstract;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.Create;

public record CreateInvoiceRequest(CreateInvoiceDTO CreateInvoiceDto) : IRequest<ErrorOr<InvoiceDTO>>;

public class CreateInvoiceHandler(
    IInvoiceRepository invoiceRepository,
    ICustomerRepository customerRepository,
    IItemRepository itemRepository,
    ICurrencyConverter currencyConverter,
    IMapper mapper) : IRequestHandler<CreateInvoiceRequest, ErrorOr<InvoiceDTO>>
{
    public async Task<ErrorOr<InvoiceDTO>> Handle(CreateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoice = mapper.Map<Data.Models.Invoice>(request.CreateInvoiceDto);

        var customerTask = customerRepository.GetByIdAsync(invoice.CustomerId, cancellationToken);
        var itemsTask = itemRepository.GetByIdsAsync(invoice.ItemIds, cancellationToken);

        await Task.WhenAll(customerTask, itemsTask);

        var conversionTasks = itemsTask.Result.GroupBy(x => x.CurrencyCode)
               .Select(group => currencyConverter.ConvertAsync(group.Sum(x => x.Price), group.Key, customerTask.Result.CurrencyCode, cancellationToken));

        invoice.Id = Guid.NewGuid();
        invoice.Amount = (await Task.WhenAll(conversionTasks)).Sum();
        invoice.CurrencyId = customerTask.Result.CurrencyId;
        invoice.Currency = customerTask.Result.CurrencyCode;

        await invoiceRepository.AddAsync(invoice, cancellationToken);
        return mapper.Map<InvoiceDTO>(invoice);
    }
}
