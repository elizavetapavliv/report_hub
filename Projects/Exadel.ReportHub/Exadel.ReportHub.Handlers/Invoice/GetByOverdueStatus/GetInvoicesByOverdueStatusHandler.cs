using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.GetByOverdueStatus;

public record GetInvoicesByOverdueStatusRequest(Guid ClientId) : IRequest<ErrorOr<OverdueInvoicesResultDTO>>;

public class GetInvoicesByOverdueStatusHandler(IInvoiceRepository invoiceRepository, IMapper mapper) : IRequestHandler<GetInvoicesByOverdueStatusRequest, ErrorOr<OverdueInvoicesResultDTO>>
{
    public async Task<ErrorOr<OverdueInvoicesResultDTO>> Handle(GetInvoicesByOverdueStatusRequest request, CancellationToken cancellationToken)
    {
        var overdueResult = await invoiceRepository.GetOverdueAsync(request.ClientId, cancellationToken);
        var overdueResultDto = mapper.Map<OverdueInvoicesResultDTO>(overdueResult);

        return overdueResultDto;
    }
}
