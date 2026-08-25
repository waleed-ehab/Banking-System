using BankingSystem.Application.UseCases.Accounts;
using BankingSystem.Application.UseCases.Authentication;
using BankingSystem.Domain.Enums;
using BankingSystem.Presentation.Helpers;
using BankingSystem.Presentation.Screens.MainMenu.ClientManagement;

namespace BankingSystem.Presentation.Screens.MainMenu.AccountManagement;

public class AccountManagementMenuScreen
{
    public enum AccountManagementOption
    {
        ShowAccounts = 1,
        AddNewAccount = 2,
        DeleteAccount = 3,
        FindAccount = 4,
        Exit = 0
    }

    private readonly LoginResponse _loggedInUser;
    private readonly GetAllAccountsUseCase _getAllAccounts;
    private readonly CreateAccountUseCase _createAccount;
    private readonly DeleteAccountUseCase _deleteAccount;
    private readonly GetAccountUseCase _getAccount;

    public AccountManagementMenuScreen(
        LoginResponse loggedInUser,
        GetAllAccountsUseCase getAllAccounts,
        CreateAccountUseCase createAccount,
        DeleteAccountUseCase deleteAccount,
        GetAccountUseCase getAccount)
    {
        _loggedInUser = loggedInUser;
        _getAllAccounts = getAllAccounts;
        _createAccount = createAccount;
        _deleteAccount = deleteAccount;
        _getAccount = getAccount;
    }

    public void Run()
    {
        ConsoleHelper.RunMenu(
            PrintAccountManagementMenu,
            HandleOption,
            (int)AccountManagementOption.Exit,
            (int)AccountManagementOption.FindAccount,
            option => (AccountManagementOption)option == AccountManagementOption.Exit
        );
    }

    private void PrintAccountManagementMenu()
    {
        var menuItems = new List<(int Number, string Text)>
        {
            (1, "Show Accounts"),
            (2, "Create New Account"),
            (3, "Delete Account"),
            (4, "Find Account"),
            (0, "Exit")
        };

        ConsoleHelper.PrintMenuLayout(
            "Account Management",
            () => ConsoleHelper.PrintMenu(menuItems),
            _loggedInUser.Role.ToString(),
            _loggedInUser.FullName,
            _loggedInUser.Permissions.ToString()
        );
    }

    private void HandleOption(int option)
    {
        switch ((AccountManagementOption)option)
        {
            case AccountManagementOption.ShowAccounts:

                if (!ConsoleHelper.ValidatePermission(_loggedInUser.Permissions, Permissions.Read))
                    return;

                var getAllAccounts = new GetAllAccountsScreen(_getAllAccounts);
                getAllAccounts.Run();
                break;

            case AccountManagementOption.AddNewAccount:

                if (!ConsoleHelper.ValidatePermission(_loggedInUser.Permissions, Permissions.Execute))
                    return;

                var createAccount = new CreateAccountScreen(_createAccount);
                createAccount.Run();
                break;

            case AccountManagementOption.DeleteAccount:

                if (!ConsoleHelper.ValidatePermission(_loggedInUser.Permissions, Permissions.Execute))
                    return;

                var deleteAccount = new DeleteAccountScreen(_deleteAccount, _getAccount);
                deleteAccount.Run();
                break;

            case AccountManagementOption.FindAccount:

                if (!ConsoleHelper.ValidatePermission(_loggedInUser.Permissions, Permissions.Read))
                    return;

                var getAccount = new GetAccountScreen(_getAccount);
                getAccount.Run();
                break;
        }
    }
}
