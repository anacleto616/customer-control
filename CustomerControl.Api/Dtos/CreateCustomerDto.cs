using System.ComponentModel.DataAnnotations;

namespace CustomerControl.Api.Dtos;

public record class CreateCustomerDto(
    [Required] [StringLength(50)] string Name,
    [Required] [StringLength(maximumLength: 11, MinimumLength = 11)] string Document,
    [Required] [StringLength(maximumLength: 11, MinimumLength = 10)] string Phone,
    [Required] [StringLength(50)] string Address,
    [Required] int UserId
);
