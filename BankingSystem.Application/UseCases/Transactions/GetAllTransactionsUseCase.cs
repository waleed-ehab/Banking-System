using BankingSystem.Application.Common;
using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.UseCases.Transactions;

public record TransactionSummary(string TransactionId, string AccountId, decimal Amount, string CurrencyCode, string Type, DateTime OccurredAt);

public record GetAllTransactionsResponse(IEnumerable<TransactionSummary> Transactions);

public class GetAllTransactionsUseCase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICurrentUser _currentUser;
    public GetAllTransactionsUseCase(ITransactionRepository transactionRepository, ICurrentUser currentUser)
    {
        _transactionRepository = transactionRepository;
        _currentUser = currentUser;
    }

    public GetAllTransactionsResponse Execute()
    {
        if (!_currentUser.Permissions.HasFlag(Permissions.Read))
            throw new UnauthorizedException("You do not have permission to perform this action.");

        var transactions = _currentUser.Role == UserRole.Admin
            ? _transactionRepository.GetAll()
            : _transactionRepository.GetAllByClient(_currentUser.Id);

        var summaries = transactions.Select(t => new TransactionSummary(
            t.TransactionId,
            t.AccountId,
            t.Amount.Amount,
            t.Amount.CurrencyCode,
            t.Type.ToString(),
            t.OccurredAt
        ));

        return new GetAllTransactionsResponse(summaries);
    }
}
