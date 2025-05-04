using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Handlers.Report.ExportInvoicesReport;
using Exadel.ReportHub.Handlers.Report.ExportItemsReport;
using Exadel.ReportHub.Host.Infrastructure.Models;
using Exadel.ReportHub.Host.Services.Abstract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
[Route("api/reports")]
public class ReportsService(ISender sender) : BaseService
{
    [Authorize(Policy = Constants.Authorization.Policy.Export)]
    [HttpGet("invoices/export/{format}")]
    [SwaggerOperation(Summary = "Export invoices report", Description = "Generates and exports a report of client invoices in required format using the provided client id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Report was exported successfully")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication is required to access this endpoint")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "User doesnt have permission to export the report")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Invoices were not found for the specified client id", typeof(ErrorResponse))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(ErrorResponse))]
    public async Task<ActionResult<ExportResult>> ExportInvoicesReportAsync([FromRoute] ExportFormat format, [FromQuery][Required] Guid clientId)
    {
        var result = await sender.Send(new ExportInvoicesReportRequest(clientId, format));
        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Export)]
    [HttpGet("items/export/{format}")]
    [SwaggerOperation(Summary = "Export items report", Description = "Generates and exports a report of client items in required format using the provided client id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Report was exported successfully")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication is required to access this endpoint")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "User doesnt have permission to export the report")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Items were not found for the specified client id", typeof(ErrorResponse))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(ErrorResponse))]
    public async Task<ActionResult<ExportResult>> ExportItemsReportAsync([FromRoute] ExportFormat format, [FromQuery][Required] Guid clientId)
    {
        var result = await sender.Send(new ExportItemsReportRequest(clientId, format));
        return FromResult(result);
    }
}
