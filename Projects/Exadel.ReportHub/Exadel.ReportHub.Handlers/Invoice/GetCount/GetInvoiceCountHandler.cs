using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.GetCount;

public record GetInvoiceCountRequest(InvoiceFilterDTO InvoiceFilterDto, Guid ClientId) : IRequest<ErrorOr<InvoiceCountResultDTO>>;

public class GetInvoiceCountHandler(IInvoiceRepository invoiceRepository) : IRequestHandler<GetInvoiceCountRequest, ErrorOr<InvoiceCountResultDTO>>
{
    public async Task<ErrorOr<InvoiceCountResultDTO>> Handle(GetInvoiceCountRequest request, CancellationToken cancellationToken)
    {
        var invoiceCount = await invoiceRepository.GetCountByDateRangeAsync(
            request.InvoiceFilterDto.StartDate,
            request.InvoiceFilterDto.EndDate,
            request.ClientId,
            request.InvoiceFilterDto.CustomerId,
            cancellationToken);

        return new InvoiceCountResultDTO
        {
            Total = invoiceCount.Sum(x => x.Value),
            Customers = invoiceCount
        };
    }
}
