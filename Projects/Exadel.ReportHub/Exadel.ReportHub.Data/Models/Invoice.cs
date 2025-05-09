﻿using Exadel.ReportHub.Data.Abstract;
using Exadel.ReportHub.Data.Enums;

namespace Exadel.ReportHub.Data.Models;

public class Invoice : IDocument, ISoftDeletable
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public Guid CustomerId { get; set; }

    public string InvoiceNumber { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime DueDate { get; set; }

    public string ClientBankAccountNumber { get; set; }

    public Guid ClientCurrencyId { get; set; }

    public string ClientCurrencyCode { get; set; }

    public decimal ClientCurrencyAmount { get; set; }

    public Guid CustomerCurrencyId { get; set; }

    public string CustomerCurrencyCode { get; set; }

    public decimal CustomerCurrencyAmount { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public IList<Guid> ItemIds { get; set; }

    public bool IsDeleted { get; set; }
}
