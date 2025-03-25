using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.ReportHub.Data.Models;

public class Invoice : IDocument
{
    public Guid Id { get; set; }

    public Guid InvoiceId { get; set; } = Guid.NewGuid();

    public DateTime IssueDate { get; set; }

    public DateTime DueDate { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; }

    public string PaymentStatus { get; set; }
 }
