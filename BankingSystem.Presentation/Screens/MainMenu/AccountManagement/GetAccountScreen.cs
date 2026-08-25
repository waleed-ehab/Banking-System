using BankingSystem.Application.UseCases.Accounts;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.AccountManagement;

public class GetAccountScreen
{
    private readonly GetAccountUseCase _getAccount;

    public GetAccountScreen(GetAccountUseCase getAccount)
    {
        _getAccount = getAccount;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Get Account");

        string id = ConsoleHelper.ReadInput("Enter account id: ");

        try
        {
            var account = _getAccount.Execute(new GetAccountRequest(id));

            Console.Clear();
            ConsoleHelper.PrintAccountCard(
                account.AccountId,
                account.ClientId,
                account.Balance, 
                account.CurrencyCode, 
                account.PinCode,
                account.IsLocked,
                account.IsDeleted
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        ConsoleHelper.WaitForUser();
    }
}
