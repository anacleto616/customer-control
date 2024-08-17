using CustomerControl.Api.Dtos;
using CustomerControl.Api.Entities;
using CustomerControl.Api.Enums;

namespace CustomerControl.Api.Mapping;

public static class InvoiceMapping
{
    public static InvoiceSummaryDto ToInvoiceSummaryDto(this Invoice invoice)
    {
        var invoiceStatus = invoice.Paid
            ? InvoiceStatus.PAGO
            : (DateTime.Now > invoice.DueDate ? InvoiceStatus.ATRASADO : InvoiceStatus.PENDENTE);

        return new InvoiceSummaryDto(
            invoice.Id,
            invoice.Description,
            invoice.Amount,
            invoice.DueDate,
            invoiceStatus
        );
    }
}
