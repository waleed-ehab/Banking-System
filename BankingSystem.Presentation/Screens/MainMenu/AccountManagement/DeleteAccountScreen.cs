using BankingSystem.Application.UseCases.Accounts;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.AccountManagement;

public class DeleteAccountScreen
{
    private readonly DeleteAccountUseCase _deleteAccount;
    private readonly GetAccountUseCase _getAccount;

    public DeleteAccountScreen(DeleteAccountUseCase deleteAccount, GetAccountUseCase getAccount)
    {
        _deleteAccount = deleteAccount;
        _getAccount = getAccount;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Delete Account");

        string accountId = ConsoleHelper.ReadInput("Enter account id: ");

        try
        {
            var account = _getAccount.Execute(new GetAccountRequest(accountId));

            Console.Clear();
            ConsoleHelper.PrintAccountCard(account.AccountId, account.ClientId,account.Balance, account.CurrencyCode, account.PinCode, account.IsLocked, account.IsDeleted);

            char confirmation = ConsoleHelper.ReadConfirmationChar(
                $"\nAre you sure you want to {ConsoleHelper.Color("delete", ConsoleColorCode.Red)} this account? {ConsoleHelper.Color("(Y/N)", ConsoleColorCode.Red)}: "
            );

            if (confirmation == 'y' || confirmation == 'Y')
            {
                _deleteAccount.Execute(new DeleteAccountRequest(accountId));
                Console.WriteLine("\n\nAccount has been deleted successfully.");
            }
            else
            {
                Console.WriteLine("\n\nDeletion canceled.");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        ConsoleHelper.WaitForUser();
    }
}
