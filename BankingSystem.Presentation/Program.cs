using BankingSystem.Application.Common;
using BankingSystem.Application.UseCases.Accounts;
using BankingSystem.Application.UseCases.Authentication;
using BankingSystem.Application.UseCases.Clients;
using BankingSystem.Application.UseCases.Currencies;
using BankingSystem.Application.UseCases.PermissionManagement;
using BankingSystem.Application.UseCases.Transactions;
using BankingSystem.Domain.Persistence;
using BankingSystem.Domain.Security;
using BankingSystem.Infrastructure.Persistence;
using BankingSystem.Infrastructure.Seeders;
using BankingSystem.Presentation.Screens.Login;
using BankingSystem.Presentation.Screens.MainMenu;
using System.Configuration;

try
{
    string GetConfig(string key) =>
        ConfigurationManager.AppSettings[key]
        ?? throw new Exception($"Missing {key}");

    string key = GetConfig("Security.PinEncryptionKey");

    var usersPath = GetConfig("Paths.UsersFile");
    var accountsPath = GetConfig("Paths.AccountsFile");
    var currenciesPath = GetConfig("Paths.CurrenciesFile");
    var transactionsPath = GetConfig("Paths.TransactionsFile");
    var transfersPath = GetConfig("Paths.TransfersFile");

    var userRepository = new JsonUserRepository(usersPath);
    var accountRepository = new JsonAccountRepository(accountsPath);
    var currencyRepository = new JsonCurrencyRepository(currenciesPath);
    var transactionRepository = new JsonTransactionRepository(transactionsPath);
    var transferRepository = new JsonTransferRepository(transfersPath);
    
    var passwordHasher = new BcryptPasswordHasher();
    var aesEncrypter = new AesEncryptionService(key);

    var convertCurrency = new ConvertCurrencyUseCase(currencyRepository);
    var getAllCurrencies = new GetAllCurrenciesUseCase(currencyRepository);
    var getCurrency = new GetCurrencyUseCase(currencyRepository);
    var login = new LoginUseCase(userRepository, passwordHasher);

    var adminSeeder = new AdminSeeder(userRepository, passwordHasher);
    var currencySeeder = new CurrencySeeder(currencyRepository);

    adminSeeder.Seed();
    currencySeeder.Seed();

    var loginScreen = new LoginScreen(login);

    while (true)
    {
        var loggedInUser = loginScreen.Run();

        if (loggedInUser is null)
        {
            Console.WriteLine("\n\nThank you for using the application. Developed by Waleed Ehab.");
            return;
        }

        var currentUser = new CurrentUser(loggedInUser.Id, loggedInUser.Role, loggedInUser.Permissions);

        var deposit = new DepositUseCase(
            accountRepository,
            transactionRepository,
            aesEncrypter,
            currentUser
        );

        var withdraw = new WithdrawUseCase(
            accountRepository,
            transactionRepository,
            aesEncrypter,
            currentUser
        );

        var transfer = new TransferUseCase(
            accountRepository,
            transferRepository,
            currentUser
        );

        var getAllTransactions = new GetAllTransactionsUseCase(
            transactionRepository,
            currentUser
        );

        var getAllTransfers = new GetAllTransfersUseCase(
            accountRepository,
            transferRepository,
            currentUser
        );

        var createAccount = new CreateAccountUseCase(
            userRepository,
            currencyRepository,
            accountRepository,
            aesEncrypter,
            currentUser
        );

        var getAccount = new GetAccountUseCase(
            accountRepository,
            aesEncrypter,
            currentUser
        );

        var getAllAccounts = new GetAllAccountsUseCase(
            accountRepository,
            aesEncrypter,
            currentUser
        );

        var deleteAccount = new DeleteAccountUseCase(
            accountRepository,
            currentUser
        );

        var registerClient = new RegisterClientUseCase(
            userRepository,
            passwordHasher,
            currentUser
        );

        var deleteClient = new DeleteClientUseCase(
            userRepository, 
            accountRepository,
            currentUser
        );

        var updateClient = new UpdateClientUseCase(
            userRepository,
            currentUser
        );

        var getClient = new GetClientUseCase(
            userRepository, 
            currentUser
        );

        var getAllClients = new GetAllClientsUseCase(
            userRepository,
            currentUser
        );

        var updateCurrency = new UpdateCurrencyUseCase(
            currencyRepository,
            currentUser
        );

        var grantPermission = new GrantPermissionUseCase(
            userRepository,
            currentUser
        );

        var revokePermission = new RevokePermissionUseCase(
            userRepository,
            currentUser
        );

        var getClientPermissions = new GetClientPermissionsUseCase(
            userRepository,
            currentUser
        );

        var resetClientPermissions = new ResetClientPermissionsUseCase(
            userRepository,
            currentUser
        );

        var mainMenu = new MainMenuScreen(
            loggedInUser,
            registerClient,
            deleteClient,
            updateClient,
            getClient,
            getCurrency,
            getAllClients,
            getAllCurrencies,
            updateCurrency,
            convertCurrency,
            getAllAccounts,
            getAccount,
            createAccount,
            deleteAccount,
            deposit,
            withdraw,
            transfer,
            grantPermission,
            revokePermission,
            getClientPermissions,
            resetClientPermissions,
            getAllTransactions,
            getAllTransfers
        );

        mainMenu.Run();
    }

}
catch (Exception ex)
{
    Console.WriteLine($"\nError: {ex.Message}");
}
