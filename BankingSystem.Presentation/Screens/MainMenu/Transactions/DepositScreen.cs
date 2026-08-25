using BankingSystem.Application.UseCases.Transactions;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.Transactions;

public class DepositScreen
{
    private readonly DepositUseCase _deposit;

    public DepositScreen(DepositUseCase deposit)
    {
        _deposit = deposit;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Deposit");

        string accountId = ConsoleHelper.ReadInput("Enter account ID: ");
        decimal amount = ConsoleHelper.ReadDecimal("Enter deposit amount: ");

        try
        {
            var deposit = _deposit.Execute(new DepositRequest(accountId, amount));

            Console.Clear();
            ConsoleHelper.PrintAccountCard(deposit.AccountId, deposit.ClientId, deposit.Balance, deposit.CurrencyCode, deposit.PinCode, deposit.IsLocked, deposit.IsDeleted);

            Console.WriteLine("\nDeposit completed successfully.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        ConsoleHelper.WaitForUser();
    }
}