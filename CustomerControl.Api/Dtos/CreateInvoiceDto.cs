using System.ComponentModel.DataAnnotations;

namespace CustomerControl.Api.Dtos;

public record class CreateInvoiceDto(
    [Required] [StringLength(50)] string Description,
    [Required] decimal Amount,
    [Required] DateTime DueDate,
    [Required] bool Paid,
    [Required] int CustomerId
);
