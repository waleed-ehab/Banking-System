using BankingSystem.Application.UseCases.Transactions;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.Transactions;

public class GetAllTransfersScreen
{
    private readonly GetAllTransfersUseCase _getAllTransfers;

    public GetAllTransfersScreen(GetAllTransfersUseCase getAllTransfers)
    {
        _getAllTransfers = getAllTransfers;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Transfer Logs");

        var result = _getAllTransfers.Execute();

        if (!result.Transfers.Any())
        {
            ConsoleHelper.PrintCenteredHeader("---- There are currently no transfers in the system ----", 45, 26, false);
        }
        else
        {
            int sourceAccountIdW = Math.Max("Source Account Id".Length, result.Transfers.Max(c => c.SourceAccountId.Length)) + 2;
            int destinationAccountIdW = Math.Max("Destination Account Id".Length, result.Transfers.Max(c => c.DestinationAccountId.Length)) + 2;
            int amountW = Math.Max("Amount".Length, result.Transfers.Max(c => c.Amount.ToString().Length)) + 2;
            int currencyCodeW = Math.Max("Currency Code".Length, result.Transfers.Max(c => c.CurrencyCode.Length)) + 2;
            int occurredAtW = Math.Max("Occurred At".Length, result.Transfers.Max(c => c.OccurredAt.ToString().Length)) + 2;
            int borderW = sourceAccountIdW + destinationAccountIdW + amountW + currencyCodeW + occurredAtW + 6;

            Console.WriteLine($"{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
            Console.WriteLine(
                $"{ConsoleHelper.GetTabs(1)}" +
                $"|{ConsoleHelper.Color("Source Account Id".PadRight(sourceAccountIdW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Destination Account Id".PadRight(destinationAccountIdW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Amount".PadRight(amountW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Currency Code".PadRight(currencyCodeW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Occurred At".PadRight(occurredAtW), ConsoleColorCode.Cyan)}|"
            );

            foreach (var transction in result.Transfers)
            {
                Console.WriteLine($"{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
                Console.WriteLine(
                    $"{ConsoleHelper.GetTabs(1)}" +
                    $"|{transction.SourceAccountId.PadRight(sourceAccountIdW)}" +
                    $"|{transction.DestinationAccountId.PadRight(destinationAccountIdW)}" +
                    $"|{transction.Amount.ToString().PadRight(amountW)}" +
                    $"|{transction.CurrencyCode.PadRight(currencyCodeW)}" +
                    $"|{transction.OccurredAt.ToString().PadRight(occurredAtW)}|"
                );
            }

            Console.WriteLine($"{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
        }

        ConsoleHelper.WaitForUser();

    }
}
