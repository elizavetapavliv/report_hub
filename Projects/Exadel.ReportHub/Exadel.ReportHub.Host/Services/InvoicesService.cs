using System.Diagnostics.CodeAnalysis;
using System.IO;
using Exadel.ReportHub.Handlers.Invoice.Export;
using Exadel.ReportHub.Handlers.Invoice.Import;
using Exadel.ReportHub.Host.Services.Abstract;
using Exadel.ReportHub.SDK.DTOs.Import;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
[Route("api/invoices")]
public class InvoicesService(ISender sender) : BaseService
{
    [Authorize(Policy = Constants.Authorization.Policy.Create)]
    [HttpPost("import")]
    public async Task<ActionResult<ImportResultDTO>> ImportInvoicesAsync([FromForm] ImportDTO importDto)
    {
        var result = await sender.Send(new ImportInvoicesRequest(importDto));

        return FromResult(result);
    }

    [HttpGet("export")]
    public async Task<ActionResult<Stream>> ExportInvoiceAsync(Guid invoiceId)
    {
        var stream = await sender.Send(new ExportInvoiceRequest(invoiceId));
        var result = new FileStreamResult(stream.MemoryStream, "application/pdf")
        {
            FileDownloadName = stream.FileName
        };

        return result;
    }
}
