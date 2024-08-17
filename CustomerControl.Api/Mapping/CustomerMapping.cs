using CustomerControl.Api.Dtos;
using CustomerControl.Api.Entities;

namespace CustomerControl.Api.Mapping;

public static class CustomerMapping
{
    public static Customer ToEntity(this CreateCustomerDto customer)
    {
        return new Customer()
        {
            Name = customer.Name,
            Document = customer.Document,
            Phone = customer.Phone,
            Address = customer.Address,
            UserId = customer.UserId,
        };
    }

    public static Customer ToEntity(this UpdateCustomerDto customer, int id)
    {
        return new Customer()
        {
            Id = id,
            Name = customer.Name,
            Document = customer.Document,
            Phone = customer.Phone,
            Address = customer.Address,
            UserId = customer.UserId,
        };
    }

    public static CustomerDetailsDto ToCustomerDetailsDto(this Customer customer)
    {
        var paidInvoicesCount = customer.Invoices?.Count(invoice => invoice.Paid) ?? 0;
        var openInvoicesCount = customer.Invoices?.Count(invoice => !invoice.Paid) ?? 0;
        var overdueInvoicesCount =
            customer.Invoices?.Count(invoice => invoice.DueDate < DateTime.Now) ?? 0;

        return new CustomerDetailsDto(
            customer.Id,
            customer.Name,
            paidInvoicesCount,
            openInvoicesCount,
            overdueInvoicesCount
        );
    }

    public static CustomerSummaryDto ToCustomerSummaryDto(this Customer customer)
    {
        return new CustomerSummaryDto(
            customer.Id,
            customer.Name,
            customer.Document,
            customer.Phone,
            customer.Address
        );
    }
}
