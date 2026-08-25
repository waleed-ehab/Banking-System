using BankingSystem.Domain.Exceptions;

namespace BankingSystem.Domain.ValueObjects;

public class Username
{
    public string Value { get; }

    private Username(string value)
    {
        Value = value;
    }

    public static Username Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Username cannot be empty.");

        if (value.Length < 4)
            throw new DomainException("Username must be at least 4 characters.");

        if (value.Length > 20)
            throw new DomainException("Username cannot exceed 20 characters.");

        if (!value.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-'))
            throw new DomainException("Username can only contain letters, numbers, underscores and hyphens.");

        if (value.Count(c => char.IsLetterOrDigit(c)) < 3)
            throw new DomainException("Username must be at least 3 letters or digits.");

        return new Username(value.ToLowerInvariant());
    }

    public override bool Equals(object? obj)
    {
        return obj is Username other && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }
}
