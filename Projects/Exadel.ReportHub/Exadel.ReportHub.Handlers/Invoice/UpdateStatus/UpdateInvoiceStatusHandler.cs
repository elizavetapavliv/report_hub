using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.Enums;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.UpdateStatus;

public record UpdateInvoiceStatusRequest(Guid Id, Guid ClientId) : IRequest<ErrorOr<Updated>>;

public class UpdateInvoiceStatusHandler(IInvoiceRepository invoiceRepository) : IRequestHandler<UpdateInvoiceStatusRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateInvoiceStatusRequest request, CancellationToken cancellationToken)
    {
        var isExists = await invoiceRepository.ExistsAsync(request.Id, cancellationToken);
        if (!isExists)
        {
            return Error.NotFound();
        }

        await invoiceRepository.UpdatePaidStatusAsync(request.Id, request.ClientId, cancellationToken);

        return Result.Updated;
    }
}
