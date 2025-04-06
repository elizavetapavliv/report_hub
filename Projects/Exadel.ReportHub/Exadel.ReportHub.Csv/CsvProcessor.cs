using CsvHelper;
using Exadel.ReportHub.Data.Models;
using System.Globalization;


namespace Exadel.ReportHub.Csv;

public class CsvProcessor
{
    public IEnumerable<Invoice> ParseInvoice(Stream csvStream)
    { 
        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        return csv.GetRecords<Invoice>().ToList();
    }
}
