using BankingSystem.Application.Common;
using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.UseCases.Accounts;

public record GetAccountRequest(string AccountId);

public record GetAccountResponse(string AccountId, string ClientId, decimal Balance, string CurrencyCode, string PinCode, bool IsLocked, bool IsDeleted);

public class GetAccountUseCase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly ICurrentUser _currentUser;

    public GetAccountUseCase(IAccountRepository accountRepository, IEncryptionService encryptionService, ICurrentUser currentUser)
    {
        _accountRepository = accountRepository;
        _encryptionService = encryptionService;
        _currentUser = currentUser;
    }

    public GetAccountResponse Execute(GetAccountRequest request)
    {
        if (!_currentUser.Permissions.HasFlag(Permissions.Read))
            throw new UnauthorizedException("You do not have permission to perform this action.");

        var account = _accountRepository.GetById(request.AccountId);

        if (account is null || (_currentUser.Role == UserRole.Client && account.UserId != _currentUser.Id))
            throw new NotFoundException($"Account with id '{request.AccountId}' not found.");

        return new GetAccountResponse(
            account.AccountId,
            account.UserId,
            account.Balance.Amount,
            account.Balance.CurrencyCode,
            _encryptionService.Decrypt(account.Pin.EncryptedPin),
            account.IsLocked,
            account.IsDeleted
        );
    }
}
