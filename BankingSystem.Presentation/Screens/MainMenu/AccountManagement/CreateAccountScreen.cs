using BankingSystem.Application.UseCases.Accounts;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.ClientManagement;

public class CreateAccountScreen
{
    private readonly CreateAccountUseCase _createAccount;

    public CreateAccountScreen(CreateAccountUseCase createAccount)
    {
        _createAccount = createAccount;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Add New Account");

        var initialBalance = ConsoleHelper.ReadDecimal("Enter initial balance: ");
        var currencyCode = ConsoleHelper.ReadInput("Enter currency code: ");

        try
        {
            var result = _createAccount.Execute(new CreateAccountRequest(initialBalance, currencyCode));

            Console.Clear();
            int accountIdW = Math.Max("Account Id".Length, result.AccountId.Length) + 2;
            int firstNameW = Math.Max("First Name".Length, result.FirstName.Length) + 2;
            int lastNameW = Math.Max("Last Name".Length, result.LastName.Length) + 2;
            int balanceW = Math.Max("Balance".Length, result.Balance.ToString().Length) + 2;
            int currencyCodeW = Math.Max("Currency Code".Length, result.CurrencyCode.Length) + 2;
            int initialPinW = Math.Max("Initial Pin".Length, result.InitialPin.Length) + 2;
            int borderW = accountIdW + firstNameW + lastNameW + balanceW + currencyCodeW + initialPinW + 7;

            Console.WriteLine($"\t\t{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
            Console.WriteLine(
                $"\t\t{ConsoleHelper.GetTabs(1)}" +
                $"|{ConsoleHelper.Color("Account Id".PadRight(accountIdW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("First Name".PadRight(firstNameW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Last Name".PadRight(lastNameW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Balance".PadRight(balanceW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Currency Code".PadRight(currencyCodeW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Initial Pin".PadRight(initialPinW), ConsoleColorCode.Cyan)}|"
            );

            Console.WriteLine($"\t\t{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
            Console.WriteLine(
                $"\t\t{ConsoleHelper.GetTabs(1)}" +
                $"|{result.AccountId.PadRight(accountIdW)}" +
                $"|{result.FirstName.PadRight(firstNameW)}" +
                $"|{result.LastName.PadRight(lastNameW)}" +
                $"|{result.Balance.ToString().PadRight(balanceW)}" +
                $"|{result.CurrencyCode.PadRight(currencyCodeW)}" +
                $"|{result.InitialPin.PadRight(initialPinW)}|"
            );
            Console.WriteLine($"\t\t{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
            Console.WriteLine("\nAccount created successfully.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        ConsoleHelper.WaitForUser();
    }
}
