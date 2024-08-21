using System.ComponentModel.DataAnnotations;

namespace CustomerControl.Api.Dtos;

public record class LoginResponseDto(string Token, int UserId);
