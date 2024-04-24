using System.Security.Cryptography;
using System.Text;

namespace Market.Authentication;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password, string salt)
    {
        var saltedPassword = string.Concat(password, salt);

        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(saltedPassword);

        var hashBytes = sha256.ComputeHash(bytes);

        var builder = new StringBuilder();
        foreach (var value in hashBytes)
        {
            builder.Append(value.ToString("x2"));
        }
        return builder.ToString();
    }
}