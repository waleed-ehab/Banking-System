using BankingSystem.Application.Common;
using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.UseCases.Accounts;

public record AccountSummary(string AccountId, string UserId, decimal Balance, string CurrencyCode, string PinCode, bool IsLocked, bool IsDeleted);

public record GetAllAccountsResponse(IEnumerable<AccountSummary> Accounts);

public class GetAllAccountsUseCase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly ICurrentUser _currentUser;

    public GetAllAccountsUseCase(IAccountRepository accountRepository, IEncryptionService encryptionService, ICurrentUser currentUser)
    {
        _accountRepository = accountRepository;
        _encryptionService = encryptionService;
        _currentUser = currentUser;
    }

    public GetAllAccountsResponse Execute()
    {
        if (!_currentUser.Permissions.HasFlag(Permissions.Read))
            throw new UnauthorizedException("You do not have permission to perform this action.");

        var accounts = _currentUser.Role == UserRole.Admin
            ? _accountRepository.GetAll()
            : _accountRepository.GetAllByClient(_currentUser.Id);

        var summaries = accounts.Select(a => new AccountSummary(
            a.AccountId,
            a.UserId,
            a.Balance.Amount,
            a.Balance.CurrencyCode,
            _encryptionService.Decrypt(a.Pin.EncryptedPin),
            a.IsLocked,
            a.IsDeleted
        ));

        return new GetAllAccountsResponse(summaries);
    }
}
