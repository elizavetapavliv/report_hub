using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Handlers.Invoice.Create;
using Exadel.ReportHub.Handlers.Invoice.Delete;
using Exadel.ReportHub.Handlers.Invoice.GetByClientId;
using Exadel.ReportHub.Handlers.Invoice.GetById;
using Exadel.ReportHub.Handlers.Invoice.Import;
using Exadel.ReportHub.Handlers.Invoice.Update;
using Exadel.ReportHub.Host.Infrastructure.Models;
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
    /// <summary>
    /// Imports invoices from a csv file.
    /// </summary>
    /// <param name="importDto"></param>
    /// <returns></returns>
    [Authorize(Policy = Constants.Authorization.Policy.Create)]
    [HttpPost("import")]
    public async Task<ActionResult<ImportResultDTO>> ImportInvoicesAsync([FromForm] ImportDTO importDto)
    {
        var result = await sender.Send(new ImportInvoicesRequest(importDto));

        return FromResult(result);
    }

    /// <summary>
    /// Creates a new invoice.
    /// </summary>
    /// <param name="invoiceDto"></param>
    /// <returns>The newly created invoice details.</returns>
    /// <response code="200">Invoice created successfully.</response>
    /// <response code="400">Invalid input provided.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. User does not have permission to perform this action.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Create)]
    [HttpPost]
    [ProducesResponseType(typeof(InvoiceDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]

    public async Task<ActionResult<InvoiceDTO>> AddInvoice([FromBody] CreateInvoiceDTO invoiceDto)
    {
        var result = await sender.Send(new CreateInvoiceRequest(invoiceDto));
        return FromResult(result);
    }

    /// <summary>
    /// Retrieves a list of invoices for a specific client.
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns>A list of the client's invoices.</returns>
    /// <response code="200">Invoices retrieved successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. User does not have permission to view these invoices.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Read)]
    [ProducesResponseType(typeof(IList<InvoiceDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [HttpGet]
    public async Task<ActionResult<IList<InvoiceDTO>>> GetInvoicesByClientId([FromQuery][Required] Guid clientId)
    {
        var result = await sender.Send(new GetInvoicesByClientIdRequest(clientId));
        return FromResult(result);
    }

    /// <summary>
    /// Retrieves a specific invoice by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="clientId"></param>
    /// <returns>The invoice details.</returns>
    /// <response code="200">Invoice retrieved successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. User does not have permission to view this invoice.</response>
    /// <response code="404">Invoice not found.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Read)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(InvoiceDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InvoiceDTO>> GetInvoiceById([FromRoute] Guid id, [FromQuery][Required] Guid clientId)
    {
        var result = await sender.Send(new GetInvoiceByIdRequest(id));
        return FromResult(result);
    }

    /// <summary>
    /// Deletes an existing invoice by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="clientId"></param>
    /// <returns>No content.</returns>
    /// <response code="204">Invoice deleted successfully.</response>
    /// <response code="400">Invalid request parameters.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. User does not have permission to delete this invoice.</response>
    /// <response code="404">Invoice not found.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Delete)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteInvoice([FromRoute] Guid id, [FromQuery][Required] Guid clientId)
    {
        var result = await sender.Send(new DeleteInvoiceRequest(id));
        return FromResult(result);
    }

    /// <summary>
    /// Updates the details of an existing invoice.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="invoiceDto"></param>
    /// <returns>No content.</returns>
    /// <response code="204">Invoice updated successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. User does not have permission to update this invoice.</response>
    /// <response code="404">Invoice not found.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Update)]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateInvoice([FromRoute] Guid id, [FromBody] UpdateInvoiceDTO invoiceDto)
    {
        var result = await sender.Send(new UpdateInvoiceRequest(id, invoiceDto));
        return FromResult(result);
    }
}
