using BankingSystem.Domain.Common;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Domain.Entities;

public class Account
{
    private const int MaxFailedAttempts = 3;
    private const int LockoutTimeInMins = 30;

    public string AccountId { get; }
    public string UserId { get; }
    public Pin Pin { get; }
    public Balance Balance { get; private set; }
    public int FailedPinAttempts { get; private set; }
    public bool IsLocked { get; private set; }
    public DateTime? LockedUntil { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private Account(string accountId,
        Pin pin,
        string userId,
        Balance balance,
        bool isLocked,
        int failedAttempts,
        DateTime? lockedUntil,
        bool isDeleted,
        DateTime? deletedAt)
    {
        AccountId = accountId;
        Pin = pin;
        UserId = userId;
        Balance = balance;
        IsLocked = isLocked;
        FailedPinAttempts = failedAttempts;
        LockedUntil = lockedUntil;
        IsDeleted = isDeleted;
        DeletedAt = deletedAt;
    }

    public static Account Create(Pin pin, string userId, Balance balance)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new DomainException("User id cannot be empty.");

        return new Account(EntityId.Generate(), pin, userId, balance, false, 0, null, false, null);
    }

    public static Account Rehydrate(
        string accountId,
        Pin pin,
        string userId,
        Balance balance,
        bool isLocked,
        int failedAttempts,
        DateTime? lockedUntil,
        bool isDeleted,
        DateTime? deletedAt)
    {
        return new Account(accountId, pin, userId, balance, isLocked, failedAttempts, lockedUntil, isDeleted, deletedAt);
    }

    public void RegisterFailedAttempt()
    {
        FailedPinAttempts++;

        if (FailedPinAttempts >= MaxFailedAttempts)
        {
            IsLocked = true;
            LockedUntil = DateTime.Now.AddMinutes(LockoutTimeInMins);
        }
    }

    public void ResetFailedAttempts()
    {
        FailedPinAttempts = 0;
        IsLocked = false;
        LockedUntil = null;
    }

    public bool IsLockoutExpired()
    {
        return IsLocked && LockedUntil.HasValue && DateTime.Now > LockedUntil.Value;
    }

    public void Delete()
    {
        if (IsDeleted)
            throw new DomainException("Account is already deleted.");

        IsDeleted = true;
        DeletedAt = DateTime.Now;
    }

    public void Restore()
    {
        if (!IsDeleted)
            throw new DomainException("Account is not deleted.");

        IsDeleted = false;
        DeletedAt = null;
    }

    public void Deposit(Balance amount)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot deposit into a deleted account.");

        if (amount.Amount <= 0)
            throw new DomainException("Deposit must be greater than zero.");

        Balance = Balance.Add(amount);
    }

    public void Withdraw(Balance amount)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot withdraw from a deleted account.");

        if (amount.Amount <= 0)
            throw new DomainException("withdrawal must be greater than zero.");

        if (amount.IsGreaterThan(Balance))
            throw new DomainException("Insufficient funds.");

        Balance = Balance.Subtract(amount);
    }

    public void Transfer(Balance amount, Account destination)
    {
        if (amount.Amount <= 0)
            throw new DomainException("transfer must be greater than zero.");

        Withdraw(amount);
        destination.Deposit(amount);
    }
}
