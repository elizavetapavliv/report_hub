using Microsoft.AspNetCore.Http;

namespace Exadel.ReportHub.SDK.Models;

public class FileModel
{
    public IFormFile FormFile { get; set; }

    public Guid ClientId { get; set; }
}
