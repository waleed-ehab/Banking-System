using BankingSystem.Application.Common;
using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Common;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Application.UseCases.Accounts;

public record CreateAccountRequest(decimal InitialAmount, string CurrencyCode);

public record CreateAccountResponse(string AccountId, string InitialPin, string FirstName, string LastName, decimal Balance, string CurrencyCode);

public class CreateAccountUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly ICurrentUser _currentUser;

    public CreateAccountUseCase(
        IUserRepository userRepository,
        ICurrencyRepository currencyRepository,
        IAccountRepository accountRepository,
        IEncryptionService encryptionService,
        ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _currencyRepository = currencyRepository;
        _accountRepository = accountRepository;
        _encryptionService = encryptionService;
        _currentUser = currentUser;
    }

    public CreateAccountResponse Execute(CreateAccountRequest request)
    {
        if (!_currentUser.Permissions.HasFlag(Permissions.Execute))
            throw new UnauthorizedException("You do not have permission to perform this action.");

        var user = _userRepository.GetById(_currentUser.Id);

        if (user is null)
            throw new NotFoundException($"User with id '{_currentUser.Id}' not found.");

        var currency = _currencyRepository.GetByCode(request.CurrencyCode);

        if (currency is null)
            throw new NotFoundException($"Currency with code '{request.CurrencyCode}' not found.");

        var initialPin = PinCode.Generate();
        var encryptedPin = _encryptionService.Encrypt(initialPin);

        var account = Account.Create(
            Pin.FromEncryptedText(encryptedPin),
            user.Id,
            Balance.Create(request.InitialAmount, currency.Code)
        );

        _accountRepository.Save(account);

        return new CreateAccountResponse(
            account.AccountId,
            initialPin,
            user.FirstName,
            user.LastName,
            account.Balance.Amount,
            currency.Code
        );
    }
}
