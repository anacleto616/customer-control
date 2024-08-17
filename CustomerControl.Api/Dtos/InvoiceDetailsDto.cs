using CustomerControl.Api.Enums;

namespace CustomerControl.Api.Dtos;

public record class InvoiceDetailsDto(
    int Id,
    string Description,
    decimal Amount,
    DateTime DueDate,
    InvoiceStatus Status
);
