using BankingSystem.Application.Common;
using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.UseCases.Accounts;

public record DeleteAccountRequest(string AccountId);

public class DeleteAccountUseCase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUser _currentUser;

    public DeleteAccountUseCase(IAccountRepository accountRepository, ICurrentUser currentUser)
    {
        _accountRepository = accountRepository;
        _currentUser = currentUser;
    }

    public void Execute(DeleteAccountRequest request)
    {
        if (!_currentUser.Permissions.HasFlag(Permissions.Execute))
            throw new UnauthorizedException("You do not have permission to perform this action.");

        var account = _accountRepository.GetById(request.AccountId);

        if (account is null || (_currentUser.Role == UserRole.Client && account.UserId != _currentUser.Id))
            throw new NotFoundException($"Account with id '{request.AccountId}' not found.");

        _accountRepository.Delete(request.AccountId);
    }
}
