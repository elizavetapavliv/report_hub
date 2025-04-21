using AutoMapper;
using Exadel.ReportHub.Aspose.Abstract;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Export;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using Exadel.ReportHub.SDK.DTOs.Item;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.Export;

public record ExportInvoiceRequest(Guid InvoiceId) : IRequest<ExportDTO>;

public class ExportInvoiceHandler(
    IAsposeInvoiceGenerator asposeInvoiceGenerator,
    IInvoiceRepository invoiceRepository,
    IItemRepository itemRepostitory,
    IClientRepository clientRepostitory,
    ICustomerRepository customerRepository,
    IMapper mapper) : IRequestHandler<ExportInvoiceRequest, ExportDTO>
{
    public async Task<ExportDTO> Handle(ExportInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoice = await invoiceRepository.GetByIdAsync(request.InvoiceId, cancellationToken);
        var items = await itemRepostitory.GetByIdsAsync(invoice.ItemIds, cancellationToken);
        var client = await clientRepostitory.GetByIdAsync(invoice.ClientId, cancellationToken);
        var customer = await customerRepository.GetByIdAsync(invoice.CustomerId, cancellationToken);

        var generateInvoiceDto = new GenerateInvoiceDTO
        {
            ClientName = client.Name,
            CustomerName = customer.Name,
            InvoiceNumber = invoice.InvoiceNumber,
            IssueDate = invoice.IssueDate,
            DueDate = invoice.DueDate,
            Amount = invoice.Amount,
            CurrencyCode = invoice.CurrencyCode,
            PaymentStatus = (SDK.Enums.PaymentStatus)invoice.PaymentStatus,
            BankAccountNumber = invoice.BankAccountNumber,
            ItemDtos = mapper.Map<IList<ItemDTO>>(items)
        };
        var memoryStream = asposeInvoiceGenerator.Generate(generateInvoiceDto);

        var exportDto = new ExportDTO
        {
            MemoryStream = memoryStream,
            FileName = $"Invoice_{invoice.InvoiceNumber}.pdf"
        };
        return exportDto;
    }
}
