namespace Exadel.ReportHub.Data.Models;

public class Invoice : IDocument
{
    public Guid Id { get; set; }

    public string InvoiceId { get; set; }

    public Guid ClientId { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime DueDate { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; }

    public string PaymentStatus { get; set; }
 }
