using Microsoft.AspNetCore.Http;

namespace Exadel.ReportHub.SDK.DTOs.Invoice;

public class ImportDTO
{
    public IFormFile FormFile { get; set; }

    public Guid ClientId { get; set; }
}
