using BankingSystem.Application.Interfaces;

namespace BankingSystem.Domain.Security;

public class BcryptPasswordHasher : IPasswordHasher
{
    public string Hash(string plainText)
    {
        return BCrypt.Net.BCrypt.HashPassword(plainText);
    }

    public bool Verify(string plainText, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(plainText, hash);
    }
}
