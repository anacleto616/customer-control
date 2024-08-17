using CustomerControl.Api.Dtos;
using CustomerControl.Api.Entities;
using CustomerControl.Api.Enums;

namespace CustomerControl.Api.Mapping;

public static class InvoiceMapping
{
    public static Invoice ToEntity(this CreateInvoiceDto invoice)
    {
        return new Invoice()
        {
            Description = invoice.Description,
            Amount = invoice.Amount,
            DueDate = invoice.DueDate,
            Paid = invoice.Paid,
            CustomerId = invoice.CustomerId,
        };
    }

    public static Invoice ToEntity(this UpdateInvoiceDto invoice, int id)
    {
        return new Invoice()
        {
            Id = id,
            Description = invoice.Description,
            Amount = invoice.Amount,
            DueDate = invoice.DueDate,
            Paid = invoice.Paid,
            CustomerId = invoice.CustomerId,
        };
    }

    public static InvoiceDetailsDto ToInvoiceDetailsDto(this Invoice invoice)
    {
        var invoiceStatus = invoice.Paid
            ? InvoiceStatus.PAGO
            : (DateTime.Now > invoice.DueDate ? InvoiceStatus.ATRASADO : InvoiceStatus.PENDENTE);

        return new InvoiceDetailsDto(
            invoice.Id,
            invoice.Description,
            invoice.Amount,
            invoice.DueDate,
            invoiceStatus
        );
    }

    public static InvoiceSummaryDto ToInvoiceSummaryDto(this Invoice invoice)
    {
        return new InvoiceSummaryDto(
            invoice.Id,
            invoice.Description,
            invoice.Amount,
            invoice.DueDate,
            invoice.Paid
        );
    }
}
