using System.Text;
using Exadel.ReportHub.Pdf.Abstract;
using Exadel.ReportHub.Pdf.Models;
using Exadel.ReportHub.SDK.DTOs.Item;
using SkiaSharp;

namespace Exadel.ReportHub.Pdf;

public class PdfInvoiceGenerator : IPdfInvoiceGenerator
{
    public Stream Generate(InvoiceModel invoice)
    {
        var stream = new MemoryStream();

        using var document = SKDocument.CreatePdf(stream);
        var canvas = document.BeginPage(Constants.Size.Page.Width, Constants.Size.Page.Height);
        canvas.Clear(SKColors.White);
        var font = new SKFont(SKTypeface.FromFamilyName(Constants.Text.TextStyle.Font), Constants.Text.TextStyle.FontSize);
        var fontTitle = new SKFont(SKTypeface.FromFamilyName(Constants.Text.TextStyle.Font, SKFontStyle.Bold), Constants.Text.TextStyle.FontSizeTitle);

        var paint = new SKPaint
        {
            IsAntialias = true,
            Color = SKColors.Black,
        };

        float y = Constants.MarginInfo.Page.Top;

        DrawTextLine($"{Constants.Text.Label.Invoice}: {invoice.PaymentStatus}", Constants.Size.Page.XCenter, ref y, SKTextAlign.Center, fontTitle, canvas, paint);

        DrawTextLine($"{Constants.Text.Label.InvoiceNumber}: {invoice.InvoiceNumber}", Constants.MarginInfo.Page.Left, ref y, SKTextAlign.Left, font, canvas, paint);
        DrawTextLine($"{Constants.Text.Label.IssueDate}: {invoice.IssueDate}", Constants.MarginInfo.Page.Left, ref y, SKTextAlign.Left, font, canvas, paint);
        DrawTextLine($"{Constants.Text.Label.DueDate}: {invoice.DueDate}", Constants.MarginInfo.Page.Left, ref y, SKTextAlign.Left, font, canvas, paint);
        DrawTextLine($"{Constants.Text.Label.ClientName}: {invoice.ClientName}", Constants.MarginInfo.Page.Left, ref y, SKTextAlign.Left, font, canvas, paint);
        DrawTextLine($"{Constants.Text.Label.CustomerName}: {invoice.CustomerName}", Constants.MarginInfo.Page.Left, ref y, SKTextAlign.Left, font, canvas, paint);
        DrawTextLine($"{Constants.Text.Label.ClientBankAccountNumber}: {invoice.ClientBankAccountNumber}", Constants.MarginInfo.Page.Left, ref y, SKTextAlign.Left, font, canvas, paint);

        y += font.Spacing;

        DrawTableHeader(ref y, SKTextAlign.Left, font, canvas, paint);

        foreach (var item in invoice.Items)
        {
            DrawTableRow(item, ref y, SKTextAlign.Left, font, ref canvas, paint, document);
        }

        y += fontTitle.Spacing;
        y += fontTitle.Spacing;

        if (y > Constants.Size.Page.Height - Constants.MarginInfo.Page.Bottom)
        {
            document.EndPage();
            canvas = document.BeginPage(Constants.Size.Page.Width, Constants.Size.Page.Height);
            canvas.Clear(SKColors.White);
            y = Constants.MarginInfo.Page.Top;
        }

        DrawTextLine($"{Constants.Text.Label.Total}: {invoice.Amount} {invoice.CurrencyCode}", Constants.MarginInfo.Page.Left, ref y, SKTextAlign.Left, fontTitle, canvas, paint);

        document.EndPage();
        document.Close();

        stream.Position = 0;

        return stream;
    }

    private void DrawTextLine(string text, float x, ref float y, SKTextAlign align, SKFont font, SKCanvas canvas, SKPaint paint)
    {
        canvas.DrawText(text, x, y, align, font, paint);

        y += font.Spacing;
    }

    private void DrawTableHeader(ref float y, SKTextAlign align, SKFont font, SKCanvas canvas, SKPaint paint)
    {
        var borderPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = Constants.BorderInfo.IvoiceTable.Border
        };

        var x = Constants.MarginInfo.Page.Left;
        var metrics = font.Metrics;
        var height = metrics.Descent - metrics.Ascent + Constants.MarginInfo.InvoiceTable.Top;

        canvas.DrawRect(x, y, Constants.Size.Table.NameWidth, height, borderPaint);
        canvas.DrawText(Constants.Text.Header.Name, x + Constants.MarginInfo.InvoiceTable.Left, y + height - Constants.MarginInfo.InvoiceTable.Bottom, align, font, paint);

        x += Constants.Size.Table.NameWidth;
        canvas.DrawRect(x, y, Constants.Size.Table.DescriptionWidth, height, borderPaint);
        canvas.DrawText(Constants.Text.Header.Description, x + Constants.MarginInfo.InvoiceTable.Left, y + height - Constants.MarginInfo.InvoiceTable.Bottom, align, font, paint);

        x += Constants.Size.Table.DescriptionWidth;
        canvas.DrawRect(x, y, Constants.Size.Table.PriceWidth, height, borderPaint);
        canvas.DrawText(Constants.Text.Header.Price, x + Constants.MarginInfo.InvoiceTable.Left, y + height - Constants.MarginInfo.InvoiceTable.Bottom, align, font, paint);

        y += height;
    }

    private void DrawTableRow(ItemDTO item, ref float y, SKTextAlign align, SKFont font, ref SKCanvas canvas, SKPaint paint, SKDocument document)
    {
        var borderPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = Constants.BorderInfo.IvoiceTable.Border
        };

        var x = Constants.MarginInfo.Page.Left;
        var descriptionLines = WrapText(item.Description, Constants.Size.Table.DescriptionWidth - Constants.MarginInfo.InvoiceTable.Left - Constants.MarginInfo.InvoiceTable.Right, font);
        var metrics = font.Metrics;
        var lineHeight = metrics.Descent - metrics.Ascent;
        var height = (descriptionLines.Count * lineHeight) + Constants.MarginInfo.InvoiceTable.Top;
        var cellYCenter = (height + lineHeight - Constants.MarginInfo.InvoiceTable.Bottom) / 2;

        if (y + height > Constants.Size.Page.Height - Constants.MarginInfo.Page.Bottom)
        {
            document.EndPage();
            canvas = document.BeginPage(Constants.Size.Page.Width, Constants.Size.Page.Height);
            canvas.Clear(SKColors.White);
            y = Constants.MarginInfo.Page.Top;
            DrawTableHeader(ref y, align, font, canvas, paint);
        }

        canvas.DrawRect(x, y, Constants.Size.Table.NameWidth, height, borderPaint);
        canvas.DrawText(item.Name, x + Constants.MarginInfo.InvoiceTable.Left, y + cellYCenter, align, font, paint);

        x += Constants.Size.Table.NameWidth;
        canvas.DrawRect(x, y, Constants.Size.Table.DescriptionWidth, height, borderPaint);
        for (int i = 0; i < descriptionLines.Count; i++)
        {
            canvas.DrawText(descriptionLines[i], x + Constants.MarginInfo.InvoiceTable.Left, y + (lineHeight * (i + 1)), align, font, paint);
        }

        x += Constants.Size.Table.DescriptionWidth;
        canvas.DrawRect(x, y, Constants.Size.Table.PriceWidth, height, borderPaint);
        canvas.DrawText($"{item.Price} {item.CurrencyCode}", x + Constants.MarginInfo.InvoiceTable.Left, y + cellYCenter, align, font, paint);

        y += height;
    }

    private List<string> WrapText(string text, float maxWidth, SKFont font)
    {
        var result = new List<string>();
        var words = text.Split(' ');
        var currentLine = new StringBuilder();

        foreach (var word in words)
        {
            if (font.MeasureText($"{currentLine} {word}") >= maxWidth)
            {
                result.Add(currentLine.ToString());
                currentLine.Clear();
                currentLine.Append(word);
                continue;
            }

            if (currentLine.Length > 0)
            {
                currentLine.Append(' ');
            }

            currentLine.Append(word);
        }

        if (currentLine.Length > 0)
        {
            result.Add(currentLine.ToString());
        }

        return result;
    }
}
