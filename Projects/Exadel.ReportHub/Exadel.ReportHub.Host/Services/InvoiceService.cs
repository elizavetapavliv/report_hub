using Exadel.ReportHub.Handlers.Invoice.UpdateOverdueStatus;
using Exadel.ReportHub.SDK.Abstract;
using MediatR;

namespace Exadel.ReportHub.Host.Services;

public class InvoiceService(ISender sender) : IInvoiceService
{
    public async Task UpdateOverdueInvoicesStatusAsync()
    {
        await sender.Send(new UpdateOverdueInvoicesStatusRequest());
    }
}
