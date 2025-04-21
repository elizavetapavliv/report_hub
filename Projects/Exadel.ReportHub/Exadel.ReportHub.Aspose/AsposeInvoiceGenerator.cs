using System.Diagnostics;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using Exadel.ReportHub.Aspose.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;

namespace Exadel.ReportHub.Aspose;

public class AsposeInvoiceGenerator : IAsposeInvoiceGenerator
{
    public MemoryStream Generate(GenerateInvoiceDTO invoiceDto)
    {
        var stream = new MemoryStream();

        var doc = new Document();
        var page = doc.Pages.Add();
        page.PageInfo.Margin = new MarginInfo(40, 40, 40, 40);

        var title = new TextFragment($"Invoice")
        {
            TextState = { FontSize = 18, FontStyle = FontStyles.Bold },
            HorizontalAlignment = HorizontalAlignment.Center
        };
        page.Paragraphs.Add(title);

        page.Paragraphs.Add(new TextFragment($"Invoice : {invoiceDto.InvoiceNumber}"));
        page.Paragraphs.Add(new TextFragment($"Date: {invoiceDto.DueDate}"));
        page.Paragraphs.Add(new TextFragment($"Client: {invoiceDto.ClientName}"));
        page.Paragraphs.Add(new TextFragment($"Customer: {invoiceDto.CustomerName}"));

        page.Paragraphs.Add(new TextFragment("\n"));

        var table = new Table
        {
            ColumnWidths = "200 60 80 80",
            DefaultCellPadding = new MarginInfo(5, 5, 5, 5),
            Border = new BorderInfo(BorderSide.All, 0.5f),
            DefaultCellBorder = new BorderInfo(BorderSide.All, 0.5f)
        };

        table.Rows.Add().Cells.Add("Name");
        table.Rows[0].Cells.Add("Desctipton");
        table.Rows[0].Cells.Add("Price");

        foreach (var item in invoiceDto.ItemDtos)
        {
            var row = table.Rows.Add();
            row.Cells.Add(item.Name);
            row.Cells.Add(item.Description);
            row.Cells.Add($"{item.Price}{item.CurrencyCode}");
        }

        var totalRow = table.Rows.Add();
        totalRow.Cells.Add(string.Empty);
        totalRow.Cells.Add("Total:");
        totalRow.Cells.Add($"{invoiceDto.Amount}{invoiceDto.CurrencyCode}");

        page.Paragraphs.Add(table);

        doc.Save(stream);
        stream.Position = 0;

        return stream;
    }
}
