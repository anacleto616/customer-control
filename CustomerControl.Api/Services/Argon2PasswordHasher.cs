namespace CustomerControl.Api.Services;

using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

public class Argon2PasswordHasher
{
    private readonly int _saltSize = 16;
    private readonly int _hashSize = 32;
    private readonly int _iterations = 4;
    private readonly int _memorySize = 65536;

    public string HashPassword(string password)
    {
        var salt = GenerateSalt();
        using var hasher = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            Iterations = _iterations,
            MemorySize = _memorySize,
        };

        var hash = hasher.GetBytes(_hashSize);
        var hashBytes = new byte[_saltSize + _hashSize];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, _saltSize);
        Buffer.BlockCopy(hash, 0, hashBytes, _saltSize, _hashSize);
        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string password, string storedHash)
    {
        var hashBytes = Convert.FromBase64String(storedHash);
        var salt = new byte[_saltSize];
        var storedHashBytes = new byte[_hashSize];

        Buffer.BlockCopy(hashBytes, 0, salt, 0, _saltSize);
        Buffer.BlockCopy(hashBytes, _saltSize, storedHashBytes, 0, _hashSize);

        using var hasher = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            Iterations = _iterations,
            MemorySize = _memorySize,
        };

        var newHashBytes = hasher.GetBytes(_hashSize);
        return newHashBytes.SequenceEqual(storedHashBytes);
    }

    private byte[] GenerateSalt()
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[_saltSize];
        rng.GetBytes(salt);
        return salt;
    }
}
