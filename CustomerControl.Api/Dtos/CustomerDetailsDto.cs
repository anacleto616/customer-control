namespace CustomerControl.Api.Dtos;

public record class CustomerDetailsDto(
    string Name,
    int PaidInvoices,
    int OpenInvoices,
    int OverdueInvoices
);
