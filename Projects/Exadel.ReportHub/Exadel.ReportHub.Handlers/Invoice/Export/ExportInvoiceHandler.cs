using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.Pdf.Abstract;
using Exadel.ReportHub.Pdf.Models;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Item;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.Export;

public record ExportInvoiceRequest(Guid InvoiceId) : IRequest<ErrorOr<ExportResult>>;

public class ExportInvoiceHandler(
    IAsposeInvoiceGenerator asposeInvoiceGenerator,
    IInvoiceRepository invoiceRepository,
    IItemRepository itemRepostitory,
    IClientRepository clientRepostitory,
    ICustomerRepository customerRepository,
    IMapper mapper) : IRequestHandler<ExportInvoiceRequest, ErrorOr<ExportResult>>
{
    public async Task<ErrorOr<ExportResult>> Handle(ExportInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoice = await invoiceRepository.GetByIdAsync(request.InvoiceId, cancellationToken);
        if (invoice is null)
        {
            return Error.NotFound();
        }

        var itemsTask = itemRepostitory.GetByIdsAsync(invoice.ItemIds, cancellationToken);
        var clientTask = clientRepostitory.GetByIdAsync(invoice.ClientId, cancellationToken);
        var customerTask = customerRepository.GetByIdAsync(invoice.CustomerId, cancellationToken);
        await Task.WhenAll(itemsTask, clientTask, customerTask);

        var generateInvoiceDto = new InvoiceModel
        {
            ClientName = clientTask.Result.Name,
            CustomerName = customerTask.Result.Name,
            InvoiceNumber = invoice.InvoiceNumber,
            IssueDate = invoice.IssueDate,
            DueDate = invoice.DueDate,
            Amount = invoice.Amount,
            CurrencyCode = invoice.CurrencyCode,
            PaymentStatus = (SDK.Enums.PaymentStatus)invoice.PaymentStatus,
            BankAccountNumber = invoice.BankAccountNumber,
            ItemDtos = mapper.Map<IList<ItemDTO>>(itemsTask.Result)
        };
        var stream = await asposeInvoiceGenerator.GenerateAsync(generateInvoiceDto, cancellationToken);

        var exportDto = new ExportResult
        {
            Stream = stream,
            FileName = $"{Constants.File.Name.Invoice}{invoice.InvoiceNumber}{Constants.File.Extension.Pdf}"
        };
        return exportDto;
    }
}
