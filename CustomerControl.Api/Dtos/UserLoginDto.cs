using System.ComponentModel.DataAnnotations;

namespace CustomerControl.Api.Dtos;

public record class UserLoginDto([Required] string Email, [Required] string Password);
