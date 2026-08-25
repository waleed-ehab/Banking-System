using BankingSystem.Application.UseCases.Accounts;
using BankingSystem.Application.UseCases.Authentication;
using BankingSystem.Application.UseCases.Clients;
using BankingSystem.Application.UseCases.Currencies;
using BankingSystem.Application.UseCases.PermissionManagement;
using BankingSystem.Application.UseCases.Transactions;
using BankingSystem.Domain.Enums;
using BankingSystem.Presentation.Helpers;
using BankingSystem.Presentation.Screens.MainMenu.AccountManagement;
using BankingSystem.Presentation.Screens.MainMenu.ClientManagement;
using BankingSystem.Presentation.Screens.MainMenu.CurrencyExchange;
using BankingSystem.Presentation.Screens.MainMenu.Transactions;

namespace BankingSystem.Presentation.Screens.MainMenu;

public class MainMenuScreen
{
    public enum MainMenuOption
    {
        ClientManagement = 1,
        AccountManagement = 2,
        PermissionManagement = 3,
        Transactions = 4,
        CurrencyExchange = 5,
        Logout = 0,
    }

    private readonly LoginResponse _loggedInUser;
    private readonly RegisterClientUseCase _registerClient;
    private readonly DeleteClientUseCase _deleteClient;
    private readonly UpdateClientUseCase _updateClient;
    private readonly GetClientUseCase _getClient;
    private readonly GetCurrencyUseCase _getCurrency;
    private readonly GetAllClientsUseCase _getAllClients;
    private readonly GetAllAccountsUseCase _getAllAccounts;
    private readonly CreateAccountUseCase _createAccount;
    private readonly DeleteAccountUseCase _deleteAccount;
    private readonly GetAccountUseCase _getAccount;
    private readonly GetAllCurrenciesUseCase _getAllCurrencies;
    private readonly UpdateCurrencyUseCase _updateCurrency;
    private readonly ConvertCurrencyUseCase _convertCurrency;
    private readonly DepositUseCase _deposit;
    private readonly WithdrawUseCase _withdraw;
    private readonly TransferUseCase _transfer;
    private readonly GrantPermissionUseCase _grantPermission;
    private readonly RevokePermissionUseCase _revokePermission;
    private readonly GetClientPermissionsUseCase _getClientPermissions;
    private readonly ResetClientPermissionsUseCase _resetClientPermissions;
    private readonly GetAllTransactionsUseCase _getAllTransactions;
    private readonly GetAllTransfersUseCase _getAllTransfers;

    public MainMenuScreen(
        LoginResponse loggedInUser,
        RegisterClientUseCase registerClient,
        DeleteClientUseCase deleteClient,
        UpdateClientUseCase updateClient,
        GetClientUseCase getClient,
        GetCurrencyUseCase getCurrency,
        GetAllClientsUseCase getAllClients,
        GetAllCurrenciesUseCase getAllCurrencies,
        UpdateCurrencyUseCase updateCurrency,
        ConvertCurrencyUseCase convertCurrency,
        GetAllAccountsUseCase getAllAccounts,
        GetAccountUseCase getAccount,
        CreateAccountUseCase createAccount,
        DeleteAccountUseCase deleteAccount,
        DepositUseCase deposit,
        WithdrawUseCase withdraw,
        TransferUseCase transfer,
        GrantPermissionUseCase grantPermission,
        RevokePermissionUseCase revokePermission,
        GetClientPermissionsUseCase getClientPermission,
        ResetClientPermissionsUseCase resetClientPermissions,
        GetAllTransactionsUseCase getAllTransactions,
        GetAllTransfersUseCase getAllTransfers)
    {
        _loggedInUser = loggedInUser;
        _registerClient = registerClient;
        _deleteClient = deleteClient;
        _updateClient = updateClient;
        _getClient = getClient;
        _getCurrency = getCurrency;
        _getAllClients = getAllClients;
        _getAllCurrencies = getAllCurrencies;
        _updateCurrency = updateCurrency;
        _convertCurrency = convertCurrency;
        _getAllAccounts = getAllAccounts;
        _getAccount = getAccount;
        _createAccount = createAccount;
        _deleteAccount = deleteAccount;
        _deposit = deposit;
        _withdraw = withdraw;
        _transfer = transfer;
        _grantPermission = grantPermission;
        _revokePermission = revokePermission;
        _getClientPermissions = getClientPermission;
        _resetClientPermissions = resetClientPermissions;
        _getAllTransactions = getAllTransactions;
        _getAllTransfers = getAllTransfers;
    }

    public void Run()
    {
        ConsoleHelper.RunMenu(
            PrintMainMenu,
            HandleOption,
            (int)MainMenuOption.Logout,
            (int)MainMenuOption.CurrencyExchange,
            option => (MainMenuOption)option == MainMenuOption.Logout
        );
    }

    private void HandleOption(int option)
    {
        switch ((MainMenuOption)option)
        {
            case MainMenuOption.ClientManagement:

                if (!ConsoleHelper.ValidateRole(_loggedInUser.Role, UserRole.Admin))
                    return;

                var clientManagement = new ClientManagmentMenuScreen(
                    _loggedInUser,
                    _deleteClient,
                    _getAllClients,
                    _getClient,
                    _registerClient,
                    _updateClient
                );

                clientManagement.Run();
                break;

            case MainMenuOption.CurrencyExchange:
                var currencyExchange = new CurrencyExchangeMenuScreen(
                    _loggedInUser,
                    _getAllCurrencies,
                    _getCurrency,
                    _updateCurrency,
                    _convertCurrency
                );

                currencyExchange.Run();
                break;

            case MainMenuOption.AccountManagement:
                var accountManagement = new AccountManagementMenuScreen(
                    _loggedInUser,
                    _getAllAccounts,
                    _createAccount,
                    _deleteAccount,
                    _getAccount
                );

                accountManagement.Run();
                break;

            case MainMenuOption.Transactions:
                var transactions = new TransactionsMenuScreen(
                    _loggedInUser,
                    _deposit,
                    _withdraw,
                    _transfer,
                    _getAllTransactions,
                    _getAllTransfers
                );

                transactions.Run();
                break;

            case MainMenuOption.PermissionManagement:

                if (!ConsoleHelper.ValidateRole(_loggedInUser.Role, UserRole.Admin))
                    return;

                var permissionManagement = new PermissionManagementMenuScreen(
                    _grantPermission,
                    _revokePermission,
                    _getClientPermissions,
                    _resetClientPermissions,
                    _loggedInUser
                );

                permissionManagement.Run();
                break;
        }
    }

    private void PrintMainMenu()
    {
        var menuItems = new List<(int Number, string Text)>
        {
            (1, "Client Management"),
            (2, "Account Management"),
            (3, "Permission Management"),
            (4, "Transactions"),
            (5, "Currency Exchange"),
            (0, "Logout")
        };

        ConsoleHelper.PrintMenuLayout(
            "Main Menu",
            () => ConsoleHelper.PrintMenu(menuItems),
            _loggedInUser.Role.ToString(),
            _loggedInUser.FullName,
            _loggedInUser.Permissions.ToString()
        );
    }
}

