using BankingSystem.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace BankingSystem.Domain.ValueObjects;

public class Email
{
    private static readonly Regex EmailRegex = new(
        @"^(?!.*\.\.)[a-zA-Z0-9](?:[a-zA-Z0-9._%+-]*[a-zA-Z0-9])?@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?(?:\.[a-zA-Z]{2,})+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email cannot be empty.");

        var trimmed = value.Trim();

        if (!EmailRegex.IsMatch(trimmed))
            throw new DomainException("Invalid email.");

        return new Email(trimmed);
    }

    public override bool Equals(object? obj)
    {
        return obj is Email other && Value == other.Value;
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
