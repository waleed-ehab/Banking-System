using BankingSystem.Application.UseCases.Accounts;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.AccountManagement;

public class GetAllAccountsScreen
{
    private readonly GetAllAccountsUseCase _getAllAccounts;

    public GetAllAccountsScreen(GetAllAccountsUseCase getAllAccounts)
    {
        _getAllAccounts = getAllAccounts;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Show Accounts");

        var result = _getAllAccounts.Execute();

        if (!result.Accounts.Any())
        {
            ConsoleHelper.PrintCenteredHeader("---- There are currently no accounts in the system ----", 45, 26, false);
        }
        else
        {
            int accountIdW = Math.Max("Account Id".Length, result.Accounts.Max(a => a.AccountId.Length)) + 2;
            int userIdW = Math.Max("User Id".Length, result.Accounts.Max(a => a.UserId.Length)) + 2;
            int balanceW = Math.Max("Balance".Length, result.Accounts.Max(a => a.Balance.ToString().Length)) + 2;
            int currencyCodeW = "Currency Code".Length + 2;
            int pinCodeW = "Pin Code".Length + 2;
            int isLockedW = "Is Locked".Length + 2;
            int isDeletedW = "Is Deleted".Length + 2;
            int borderW = accountIdW + userIdW + balanceW + currencyCodeW + pinCodeW + isLockedW + isDeletedW + 8;

            Console.WriteLine($"{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
            Console.WriteLine(
                $"{ConsoleHelper.GetTabs(1)}" +
                $"|{ConsoleHelper.Color("Account Id".PadRight(accountIdW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("User Id".PadRight(userIdW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Balance".PadRight(balanceW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Currency Code".PadRight(currencyCodeW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Pin Code".PadRight(pinCodeW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Is Locked".PadRight(isLockedW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Is Deleted".PadRight(isDeletedW), ConsoleColorCode.Cyan)}|"
            );

            Console.WriteLine($"{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
            foreach (var account in result.Accounts)
            {
                Console.WriteLine(
                    $"{ConsoleHelper.GetTabs(1)}" +
                    $"|{account.AccountId.PadRight(accountIdW)}" +
                    $"|{account.UserId.PadRight(userIdW)}" +
                    $"|{account.Balance.ToString().PadRight(balanceW)}" +
                    $"|{account.CurrencyCode.PadRight(currencyCodeW)}" +
                    $"|{account.PinCode.PadRight(pinCodeW)}" +
                    $"|{(account.IsLocked ? "Yes" : "No").PadRight(isLockedW)}" +
                    $"|{(account.IsDeleted ? "Yes" : "No").PadRight(isDeletedW)}|"
                );
                Console.WriteLine($"{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
            }
        }

        ConsoleHelper.WaitForUser();
    }
}