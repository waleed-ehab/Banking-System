using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.UseCases.Clients;

public record DeleteClientRequest(string Id);

public class DeleteClientUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUser _currentUser;

    public DeleteClientUseCase(IUserRepository userRepository, IAccountRepository accountRepository, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _currentUser = currentUser;
    }

    public void Execute(DeleteClientRequest request)
    {
        if (_currentUser.Role != UserRole.Admin)
            throw new UnauthorizedException("You do not have permission to perform this action.");

        var accounts = _accountRepository.GetAll();

        if (accounts.Any(a => a.UserId == request.Id))
            throw new ClientHasAccountsException("\nCannot delete client because it is linked to existing account(s).");

        var user = _userRepository.GetById(request.Id);

        if (user is not Client)
            throw new NotFoundException($"\nClient with id '{request.Id}' not found.");

        _userRepository.Delete(request.Id);
    }
}
