﻿using System.Net.Mime;
using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.Common.Providers;
using Exadel.ReportHub.Handlers.Notifications.Invoice.Export;
using Exadel.ReportHub.Pdf.Abstract;
using Exadel.ReportHub.Pdf.Models;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Item;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.ExportPdf;

public record ExportPdfInvoiceRequest(Guid InvoiceId) : IRequest<ErrorOr<ExportResult>>;

public class ExportPdfInvoiceHandler(
    IPdfInvoiceGenerator pdfInvoiceGenerator,
    IInvoiceRepository invoiceRepository,
    IItemRepository itemRepository,
    IClientRepository clientRepository,
    ICustomerRepository customerRepository,
    IUserProvider userProvider,
    IPublisher publisher,
    IMapper mapper) : IRequestHandler<ExportPdfInvoiceRequest, ErrorOr<ExportResult>>
{
    public async Task<ErrorOr<ExportResult>> Handle(ExportPdfInvoiceRequest request, CancellationToken cancellationToken)
    {
        var userId = userProvider.GetUserId();
        var isSuccess = false;

        try
        {
            var invoice = await invoiceRepository.GetByIdAsync(request.InvoiceId, cancellationToken);
            if (invoice is null)
            {
                return Error.NotFound();
            }

            var itemsTask = itemRepository.GetByIdsAsync(invoice.ItemIds, cancellationToken);
            var clientTask = clientRepository.GetByIdAsync(invoice.ClientId, cancellationToken);
            var customerTask = customerRepository.GetByIdAsync(invoice.CustomerId, cancellationToken);
            await Task.WhenAll(itemsTask, clientTask, customerTask);

            var invoiceModel = new InvoiceModel
            {
                ClientName = clientTask.Result.Name,
                CustomerName = customerTask.Result.Name,
                InvoiceNumber = invoice.InvoiceNumber,
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                Amount = invoice.Amount,
                CurrencyCode = invoice.CurrencyCode,
                PaymentStatus = (SDK.Enums.PaymentStatus)invoice.PaymentStatus,
                ClientBankAccountNumber = invoice.ClientBankAccountNumber,
                Items = mapper.Map<IList<ItemDTO>>(itemsTask.Result)
            };
            var stream = await pdfInvoiceGenerator.GenerateAsync(invoiceModel, cancellationToken);

            var exportDto = new ExportResult
            {
                Stream = stream,
                FileName = $"{Constants.File.Name.Invoice}{invoice.InvoiceNumber}{Constants.File.Extension.Pdf}",
                ContentType = MediaTypeNames.Application.Pdf
            };
            isSuccess = true;

            return exportDto;
        }
        finally
        {
            var notification = new InvoiceExportedNotification(userId, request.InvoiceId, DateTime.UtcNow, isSuccess);
            await publisher.Publish(notification, cancellationToken);
        }
    }
}
