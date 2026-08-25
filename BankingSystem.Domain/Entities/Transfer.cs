using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Domain.Entities;

public class Transfer
{
    public string SourceAccountId { get; }
    public string DestinationAccountId { get; }
    public Balance Amount { get; }
    public DateTime OccurredAt { get; }

    private Transfer(string sourceAccountId, string destinationAccountId, Balance amount, DateTime occurredAt)
    {
        SourceAccountId = sourceAccountId;
        DestinationAccountId = destinationAccountId;
        Amount = amount;
        OccurredAt = occurredAt;
    }

    public static Transfer Rehydrate(string sourceAccountId, string destinationAccountId, Balance amount, DateTime occurredAt)
    {
        return new Transfer(sourceAccountId, destinationAccountId, amount, occurredAt);
    }

    public static Transfer Create(string sourceAccountId, string destinationAccountId, Balance amount)
    {
        if (string.IsNullOrWhiteSpace(sourceAccountId))
            throw new DomainException("Account id cannot be empty.");

        if (string.IsNullOrWhiteSpace(destinationAccountId))
            throw new DomainException("Account id cannot be empty.");

        return new Transfer(
            sourceAccountId,
            destinationAccountId,
            amount,
            DateTime.Now
        );
    }
}
