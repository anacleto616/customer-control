using CustomerControl.Api.Dtos;
using CustomerControl.Api.Entities;

namespace CustomerControl.Api.Mapping;

public static class CustomerMapping
{
    public static CustomerDetailsDto ToCustomerDetailsDto(this Customer customer)
    {
        var paidInvoicesCount = customer.Invoices?.Count(invoice => invoice.Paid) ?? 0;
        var openInvoicesCount = customer.Invoices?.Count(invoice => !invoice.Paid) ?? 0;
        var overdueInvoicesCount =
            customer.Invoices?.Count(invoice => invoice.DueDate < DateTime.Now) ?? 0;

        return new CustomerDetailsDto(
            customer.Name,
            paidInvoicesCount,
            openInvoicesCount,
            overdueInvoicesCount
        );
    }
}
