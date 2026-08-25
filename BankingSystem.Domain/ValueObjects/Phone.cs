using BankingSystem.Domain.Exceptions;

namespace BankingSystem.Domain.ValueObjects;

public sealed class Phone
{
    private const int RequiredLength = 11;

    public string Value { get; }

    private Phone(string value)
    {
        Value = value;
    }

    public static Phone Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Phone cannot be empty.");

        var normalized = value.Replace(" ", "").Replace("-", "");

        if (!normalized.All(char.IsDigit))
            throw new DomainException("Phone must contain digits only.");

        if (normalized.Length != RequiredLength)
            throw new DomainException($"Phone must be exactly {RequiredLength} digits long.");

        return new Phone(normalized);
    }


    public override bool Equals(object? obj)
    {
        return obj is Phone other && Equals(other);
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