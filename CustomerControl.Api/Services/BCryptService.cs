namespace CustomerControl.Api.Services;

using BCrypt.Net;

public static class BCryptService
{
    public static string HashPassword(string password) => BCrypt.EnhancedHashPassword(password, 9);

    public static bool VerifyPassword(string password, string hashedPassword) =>
        BCrypt.EnhancedVerify(password, hashedPassword);
}
