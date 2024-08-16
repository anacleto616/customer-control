using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CustomerControl.Api.Dtos;

public record class UserRegisterDto(
    [Required] [StringLength(50)] string Name,
    [Required] [EmailAddress] string Email,
    [Required] [PasswordPropertyText] string Password
);
