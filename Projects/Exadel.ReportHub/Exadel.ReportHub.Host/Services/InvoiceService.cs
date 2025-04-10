using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Handlers.Invoice.Import;
using Exadel.ReportHub.Host.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/invoices")]
public class InvoiceService(ISender sender) : BaseService
{
    [HttpPost("import")]
    public async Task<IActionResult> ImportInvoices([FromForm] FileModel model)
    {
        using var stream = model.FormFile.OpenReadStream();
        var result = await sender.Send(new ImportInvoicesRequest(stream, model.FormFile.FileName));

        return FromResult(result);
    }
}
