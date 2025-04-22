using System.Text.RegularExpressions;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using Exadel.ReportHub.Pdf.Abstract;
using Exadel.ReportHub.Pdf.Models;
using Exadel.ReportHub.SDK.DTOs.Item;

namespace Exadel.ReportHub.Pdf;

public class AsposeInvoiceGenerator : IAsposeInvoiceGenerator
{
    public async Task<Stream> GenerateAsync(InvoiceModel invoice, CancellationToken cancellationToken)
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

        page.Paragraphs.Add(new TextFragment($"{AddSpace(nameof(invoice.InvoiceNumber))}: {invoice.InvoiceNumber}"));
        page.Paragraphs.Add(new TextFragment($"{AddSpace(nameof(invoice.IssueDate))}: {invoice.IssueDate}"));
        page.Paragraphs.Add(new TextFragment($"{AddSpace(nameof(invoice.DueDate))}: {invoice.DueDate}"));
        page.Paragraphs.Add(new TextFragment($"{AddSpace(nameof(invoice.DueDate))}: {invoice.ClientName}"));
        page.Paragraphs.Add(new TextFragment($"{AddSpace(nameof(invoice.CustomerName))}: {invoice.CustomerName}"));

        page.Paragraphs.Add(new TextFragment("\n"));

        var table = new Table
        {
            DefaultCellPadding = new MarginInfo(5, 5, 5, 5),
            Border = new BorderInfo(BorderSide.All, 0.5f),
            DefaultCellBorder = new BorderInfo(BorderSide.All, 0.5f),
            ColumnAdjustment = ColumnAdjustment.AutoFitToWindow
        };

        table.Rows.Add().Cells.Add(nameof(ItemDTO.Name));
        table.Rows[0].Cells.Add(nameof(ItemDTO.Description));
        table.Rows[0].Cells.Add(nameof(ItemDTO.Price));

        foreach (var item in invoice.ItemDtos)
        {
            var row = table.Rows.Add();
            row.Cells.Add(item.Name);
            row.Cells.Add(item.Description);
            row.Cells.Add($"{item.Price} {item.CurrencyCode}");
        }

        page.Paragraphs.Add(table);

        page.Paragraphs.Add(new TextFragment("\n"));
        var total = new TextFragment($"Total: {invoice.Amount} {invoice.CurrencyCode}")
        {
            TextState = { FontSize = 12, FontStyle = FontStyles.Bold },
            HorizontalAlignment = HorizontalAlignment.Right
        };

        page.Paragraphs.Add(total);

        await doc.SaveAsync(stream, cancellationToken);
        stream.Position = 0;

        return stream;
    }

    private string AddSpace(string text)
    {
        return Regex.Replace(text, "(?<!^)([A-Z])", " $1");
    }
}
