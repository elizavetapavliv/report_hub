using Exadel.ReportHub.Pdf;
using Exadel.ReportHub.Pdf.Abstract;

namespace Exadel.ReportHub.Host.Registrations;

public static class PdfRegistrations
{
    public static void AddPdf(this IServiceCollection services)
    {
        services.AddSingleton<IAsposeInvoiceGenerator, AsposeInvoiceGenerator>();
    }
}
