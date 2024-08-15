namespace CustomerControl.Api.Dtos;

public record class CustomerSummaryDto(
    int Id,
    string Name,
    string Document,
    string Phone,
    string Address
);
