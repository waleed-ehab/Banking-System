using BankingSystem.Application.UseCases.Transactions;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.Transactions;

public class WithdrawScreen
{
    private readonly WithdrawUseCase _withdraw;

    public WithdrawScreen(WithdrawUseCase withdraw)
    {
        _withdraw = withdraw;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Withdraw");

        string accountId = ConsoleHelper.ReadInput("Enter account ID: ");
        decimal amount = ConsoleHelper.ReadDecimal("Enter withdraw amount: ");

        try
        {
            var withdraw = _withdraw.Execute(new WithdrawRequest(accountId, amount));

            Console.Clear();
            ConsoleHelper.PrintAccountCard(
                withdraw.AccountId,
                withdraw.ClientId, 
                withdraw.Balance,
                withdraw.CurrencyCode,
                withdraw.PinCode,
                withdraw.IsLocked, 
                withdraw.IsDeleted
            );

            Console.WriteLine("\nWithdraw completed successfully.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        ConsoleHelper.WaitForUser();
    }
}