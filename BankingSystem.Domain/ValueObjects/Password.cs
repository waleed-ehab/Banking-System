using BankingSystem.Domain.Exceptions;

namespace BankingSystem.Domain.ValueObjects;

public class Password
{
    public string Hash { get; } 

    private Password(string hash)
    {
        Hash = hash;
    }

    public static Password FromHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new DomainException("Password hash cannot be empty");

        return new Password(hash);
    }

    public override bool Equals(object? obj)
    {
        return obj is Password other && Hash == other.Hash;
    }

    public override int GetHashCode()
    {
        return Hash.GetHashCode();
    }

    public override string ToString()
    {
        return Hash;
    }
}
