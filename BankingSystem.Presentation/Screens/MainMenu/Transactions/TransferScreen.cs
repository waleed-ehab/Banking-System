using BankingSystem.Application.UseCases.Transactions;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.Transactions;

public class TransferScreen
{
    private readonly TransferUseCase _transfer;

    public TransferScreen(TransferUseCase transfer)
    {
        _transfer = transfer;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Transfer");

        string sourceAccountId = ConsoleHelper.ReadInput("Enter source account id: ");
        string destinationAccountId = ConsoleHelper.ReadInput("Enter destination account id: ");
        decimal transferAmount = ConsoleHelper.ReadDecimal("Enter transfer amount: ");
        string currencyCode = ConsoleHelper.ReadInput("Enter currency code: ").ToUpper();

        try
        {
            var transfer = _transfer.Execute(new TransferRequest(sourceAccountId, destinationAccountId, transferAmount, currencyCode));

            Console.Clear();
            Console.WriteLine("\nTransfer completed successfully.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        ConsoleHelper.WaitForUser();
    }
}
