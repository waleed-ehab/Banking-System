using BankingSystem.Domain.Common;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Domain.Entities;

public class Transaction
{
    public string TransactionId { get; }
    public string AccountId { get; }
    public string ClientId { get; }
    public Balance Amount { get; }
    public TransactionType Type { get; }
    public DateTime OccurredAt { get; }

    private Transaction(string transactionId, string accountId, string clientId, Balance amount, TransactionType type, DateTime occurredAt)
    {
        TransactionId = transactionId;
        AccountId = accountId;
        ClientId = clientId;
        Amount = amount;

        Type = type;
        OccurredAt = occurredAt;
    }
    public static Transaction Rehydrate(string transactionId, string accountId, string clientId, Balance amount, TransactionType type, DateTime occurredAt)
    {
        return new Transaction(transactionId, accountId, clientId, amount, type, occurredAt);
    }

    public static Transaction Create(Balance amount, string accountId, string clientId, TransactionType type)
    {
        if (string.IsNullOrWhiteSpace(accountId))
            throw new DomainException("Account id cannot be empty.");

        return new Transaction(
            EntityId.Generate(),
            accountId,
            clientId,
            amount,
            type,
            DateTime.Now
        );
    }
}
