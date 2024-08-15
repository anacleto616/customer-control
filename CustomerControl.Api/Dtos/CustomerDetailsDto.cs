namespace CustomerControl.Api.Dtos;

public record class CustomerDetailsDto(
    int Id,
    string Name,
    int PaidInvoices,
    int OpenInvoices,
    int OverdueInvoices
);
