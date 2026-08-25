using BankingSystem.Application.UseCases.Transactions;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.Transactions;

public class GetAllTransactionsScreen
{
    private readonly GetAllTransactionsUseCase _getAllTransactions;

    public GetAllTransactionsScreen(GetAllTransactionsUseCase getAllTransactions)
    {
        _getAllTransactions = getAllTransactions;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Show Transactions");

        var result = _getAllTransactions.Execute();

        if (!result.Transactions.Any())
        {
            ConsoleHelper.PrintCenteredHeader("---- There are currently no transctions in the system ----", 45, 26, false);
        }
        else
        {
            int transactionsIdW = Math.Max("Transaction Id".Length, result.Transactions.Max(c => c.TransactionId.Length)) + 2;
            int accountIdW = Math.Max("Account Id".Length, result.Transactions.Max(c => c.AccountId.Length)) + 2;
            int amountW = Math.Max("Amount".Length, result.Transactions.Max(c => c.Amount.ToString().Length)) + 2;
            int currencyCodeW = Math.Max("Currency Code".Length, result.Transactions.Max(c => c.CurrencyCode.Length)) + 2;
            int typeW = Math.Max("Type".Length, result.Transactions.Max(c => c.Type.Length)) + 2;
            int occurredAtW = Math.Max("Occurred At".Length, result.Transactions.Max(c => c.OccurredAt.ToString().Length)) + 2;
            int borderW = transactionsIdW + accountIdW + amountW + currencyCodeW + typeW + occurredAtW + 7;

            Console.WriteLine($"{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
            Console.WriteLine(
                $"{ConsoleHelper.GetTabs(1)}" +
                $"|{ConsoleHelper.Color("Transaction Id".PadRight(transactionsIdW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Account Id".PadRight(accountIdW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Amount".PadRight(amountW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Currency Code".PadRight(currencyCodeW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Type".PadRight(typeW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Occurred At".PadRight(occurredAtW), ConsoleColorCode.Cyan)}|"
            );

            foreach (var transction in result.Transactions)
            {
                Console.WriteLine($"{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
                Console.WriteLine(
                    $"{ConsoleHelper.GetTabs(1)}" +
                    $"|{transction.TransactionId.PadRight(transactionsIdW)}" +
                    $"|{transction.AccountId.PadRight(accountIdW)}" +
                    $"|{transction.Amount.ToString().PadRight(amountW)}" +
                    $"|{transction.CurrencyCode.PadRight(currencyCodeW)}" +
                    $"|{transction.Type.PadRight(typeW)}" +
                    $"|{transction.OccurredAt.ToString().PadRight(occurredAtW)}|"
                );
            }

            Console.WriteLine($"{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
        }

        ConsoleHelper.WaitForUser();
    }
}
