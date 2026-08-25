using BankingSystem.Application.Common;
using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Application.UseCases.Transactions;

public record TransferRequest(string SourceAccountId, string DestinationAccountId, decimal Amount, string CurrencyCode);
public record TransferResponse(string SourceAccountId, string DestinationAccountId, decimal Amount, string CurrencyCode, DateTime Date);

public class TransferUseCase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransferRepository _transferRepository;
    private readonly ICurrentUser _currentUser;

    public TransferUseCase(IAccountRepository accountRepository, ITransferRepository transferRepository, ICurrentUser currentUser)
    {
        _accountRepository = accountRepository;
        _transferRepository = transferRepository;
        _currentUser = currentUser;
    }

    public TransferResponse Execute(TransferRequest request)
    {
        if (!_currentUser.Permissions.HasFlag(Permissions.Execute))
            throw new UnauthorizedException("You do not have permission to perform this action.");

        var sourceAccount = _accountRepository.GetById(request.SourceAccountId);

        if (sourceAccount is null || (_currentUser.Role == UserRole.Client && sourceAccount.UserId != _currentUser.Id))
            throw new KeyNotFoundException($"Source account with id '{request.SourceAccountId}' not found.");

        var destinationAccount = _accountRepository.GetById(request.DestinationAccountId);

        if (destinationAccount is null)
            throw new KeyNotFoundException($"Destination account with id '{request.DestinationAccountId}' not found.");

        if (sourceAccount.AccountId == destinationAccount.AccountId)
            throw new InvalidOperationException("Cannot transfer money to the same account.");

        var transferAmount = Balance.Create(request.Amount, request.CurrencyCode);
        sourceAccount.Transfer(transferAmount, destinationAccount);

        var transfer = Transfer.Create(sourceAccount.AccountId, destinationAccount.AccountId, transferAmount);

        _accountRepository.Save(sourceAccount);
        _accountRepository.Save(destinationAccount);
        _transferRepository.Save(transfer);

        return new TransferResponse(
            sourceAccount.AccountId,
            destinationAccount.AccountId,
            transferAmount.Amount,
            transferAmount.CurrencyCode,
            transfer.OccurredAt
        );
    }
}
