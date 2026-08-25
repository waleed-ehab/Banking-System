using BankingSystem.Domain.Exceptions;

namespace BankingSystem.Domain.ValueObjects;

public class Balance
{
    public decimal Amount { get; }
    public string CurrencyCode { get; }

    private Balance(decimal amount, string currencyCode)
    {
        Amount = amount;
        CurrencyCode = currencyCode;
    }

    public static Balance Create(decimal amount, string currencyCode)
    {
        if (amount < 0)
            throw new DomainException("Amount cannot be negative.");

        if (amount > 10000)
            throw new DomainException("Amount cannot exceed 10,000");

        if (string.IsNullOrWhiteSpace(currencyCode) || currencyCode.Length != 3)
            throw new DomainException("Currency code must contain exactly 3 letters (e.g. EGP, USD).");

        return new Balance(Math.Round(amount, 2), currencyCode);
    }

    public Balance Add(Balance balance)
    {
        EnsureSameCurrency(balance);

        if (balance.Amount < 0)
            throw new DomainException("Balance cannot be negative.");

        return new Balance(Math.Round(Amount + balance.Amount, 2), CurrencyCode);
    }

    public Balance Subtract(Balance balance)
    {
        EnsureSameCurrency(balance);

        if (balance.Amount > Amount)
            throw new DomainException("Insufficient balance.");

        return new Balance(Math.Round(Amount - balance.Amount, 2), CurrencyCode);
    }

    public bool IsGreaterThan(Balance other)
    {
        EnsureSameCurrency(other);
        return Amount > other.Amount;
    }

    public override bool Equals(object? obj)
    {
        return obj is Balance other && Amount == other.Amount && CurrencyCode == other.CurrencyCode;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, CurrencyCode);
    }

    public override string ToString()
    {
        return $"{Amount:F2} {CurrencyCode}";
    }

    private void EnsureSameCurrency(Balance other)
    {
        if (this.CurrencyCode != other.CurrencyCode)
            throw new DomainException("Incompatible currency.");
    }
}
