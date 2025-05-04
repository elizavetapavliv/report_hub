using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Handlers.Report.ExportInvoicesReport;
using Exadel.ReportHub.Host.Infrastructure.Models;
using Exadel.ReportHub.Host.Services.Abstract;
using Exadel.ReportHub.SDK.DTOs.Export;
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
    [HttpPost("invoices/export")]
    [SwaggerOperation(Summary = "Export invoices report", Description = "Generates and exports a report of client invoices in required format using the provided client id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Report was exported successfully")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication is required to access this endpoint")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "User doesnt have permission to export the invoice")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Invoice was not found for the specified id", typeof(ErrorResponse))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(ErrorResponse))]
    public async Task<ActionResult<ExportResult>> ExportInvoicesReportAsync([FromBody] ExportDTO exportDto)
    {
        var result = await sender.Send(new ExportInvoicesReportRequest(exportDto));
        return FromResult(result);
    }
}
