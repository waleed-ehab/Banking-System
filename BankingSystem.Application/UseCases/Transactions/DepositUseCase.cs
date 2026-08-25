using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Application.UseCases.Transactions;

public record DepositRequest(string AccountId, decimal Amount);

public record DepositResponse(string AccountId, string ClientId, decimal Balance, string CurrencyCode, string PinCode, bool IsLocked, bool IsDeleted);

public class DepositUseCase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly ICurrentUser _currentUser;

    public DepositUseCase(
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        IEncryptionService encryptionService,
        ICurrentUser currentUser)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _encryptionService = encryptionService;
        _currentUser = currentUser;
    }

    public DepositResponse Execute(DepositRequest request)
    {
        if (!_currentUser.Permissions.HasFlag(Permissions.Execute))
            throw new UnauthorizedException("You do not have permission to perform this action.");

        var account = _accountRepository.GetById(request.AccountId);

        if (account is null || (_currentUser.Role == UserRole.Client && account.UserId != _currentUser.Id))
            throw new DomainException($"Account with id '{request.AccountId}' not found.");

        var amount = Balance.Create(request.Amount, account.Balance.CurrencyCode);

        account.Deposit(amount);

        var transaction = Transaction.Create(amount, account.AccountId, account.UserId, TransactionType.Deposit);

        _accountRepository.Save(account);
        _transactionRepository.Save(transaction);

        return new DepositResponse(
            account.UserId,
            account.AccountId,
            account.Balance.Amount,
            account.Balance.CurrencyCode,
            _encryptionService.Decrypt(account.Pin.EncryptedPin),
            account.IsLocked,
            account.IsDeleted
        );
    }
}
