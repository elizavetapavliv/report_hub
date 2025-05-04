using DnsClient.Internal;
using Exadel.ReportHub.RA.Abstract;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Exadel.ReportHub.Handlers.Invoice.UpdateOverdueStatus;

public record UpdateOverdueInvoicesStatusRequest : IRequest<Unit>;

public class UpdateOverdueInvoicesStatusHandler(IInvoiceRepository invoiceRepository, ILogger<UpdateOverdueInvoicesStatusHandler> logger) : IRequestHandler<UpdateOverdueInvoicesStatusRequest, Unit>
{
    public async Task<Unit> Handle(UpdateOverdueInvoicesStatusRequest request, CancellationToken cancellationToken)
    {
        var result = await invoiceRepository.UpdateOverdueStatusAsync(cancellationToken);
        logger.LogInformation(Constants.Logger.Invoice.OverdueInvoicesResult, result);

        return Unit.Value;
    }
}
