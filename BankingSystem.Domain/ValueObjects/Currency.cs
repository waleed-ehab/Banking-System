using BankingSystem.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Domain.ValueObjects;

public class Currency
{
    public string Country { get; }
    public string Code { get; }
    public string Name { get; }
    public decimal Rate { get; }

    private Currency(string country, string code, string name, decimal rate)
    {
        Country = country;
        Code = code;
        Name = name;
        Rate = rate;
    }

    public static Currency Create(string country, string code, string name, decimal rate)
    {
        if (string.IsNullOrWhiteSpace(country))
        {
            throw new DomainException("Country cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(code) || code.Length != 3)
        {
            throw new DomainException("Currency code must contain exactly 3 letters (e.g. EGP, USD).");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name cannot be empty.");
        }

        if (rate <= 0)
        {
            throw new ValidationException("Exchange rate must be greater than zero.");
        }

        if (rate > 100000)
        {
            throw new ValidationException("Exchange rate is outside acceptable range.");
        }

        if (decimal.Round(rate, 6) != rate)
        {
            throw new ValidationException("Exchange rate cannot have more than 6 decimal places.");
        }

        return new Currency(country, code.ToUpper(), name, rate);
    }

    public static Currency Rehydrate(string country, string code, string name, decimal rate)
    {
        return new Currency(country, code, name, rate);
    }

    public override bool Equals(object? obj)
    {
        return obj is Currency other && Code == other.Code;
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }

    public override string ToString()
    {
        return $"{Code}: {Rate}";
    }
}
