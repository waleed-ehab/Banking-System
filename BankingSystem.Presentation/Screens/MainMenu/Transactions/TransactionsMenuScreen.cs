using BankingSystem.Application.UseCases.Authentication;
using BankingSystem.Application.UseCases.Transactions;
using BankingSystem.Domain.Enums;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.Transactions;

public class TransactionsMenuScreen
{
    public enum TransactionOption
    {
        Deposit = 1,
        Withdraw = 2,
        Transfer = 3,
        ShowTransactions = 4,
        TransferLogs = 5,
        Exit = 0
    }

    private readonly LoginResponse _loggedInUser;
    private readonly DepositUseCase _deposit;
    private readonly WithdrawUseCase _withdraw;
    private readonly TransferUseCase _transfer;
    private readonly GetAllTransactionsUseCase _getAllTransactions;
    private readonly GetAllTransfersUseCase _getAllTransfers;

    public TransactionsMenuScreen(
        LoginResponse loggedInUser,
        DepositUseCase deposit,
        WithdrawUseCase withdraw,
        TransferUseCase transfer,
        GetAllTransactionsUseCase getAllTransactions,
        GetAllTransfersUseCase getAllTransfers)
    {
        _loggedInUser = loggedInUser;
        _deposit = deposit;
        _withdraw = withdraw;
        _transfer = transfer;
        _getAllTransactions = getAllTransactions;
        _getAllTransfers = getAllTransfers;
    }

    public void Run()
    {
        ConsoleHelper.RunMenu(
            PrintAccountManagementMenu,
            HandleOption,
            (int)TransactionOption.Exit,
            (int)TransactionOption.TransferLogs,
            option => (TransactionOption)option == TransactionOption.Exit
        );
    }

    private void PrintAccountManagementMenu()
    {
        var menuItems = new List<(int Number, string Text)>
        {
            (1, "Deposit"),
            (2, "Withdraw"),
            (3, "Transfer"),
            (4, "Show Transactions"),
            (5, "Transfer Logs"),
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
        switch ((TransactionOption)option)
        {
            case TransactionOption.Deposit:

                if (!ConsoleHelper.ValidatePermission(_loggedInUser.Permissions, Permissions.Execute))
                    return;

                var deposit = new DepositScreen(_deposit);
                deposit.Run();
                break;

            case TransactionOption.Withdraw:

                if (!ConsoleHelper.ValidatePermission(_loggedInUser.Permissions, Permissions.Execute))
                    return;

                var withdraw = new WithdrawScreen(_withdraw);
                withdraw.Run();
                break;

            case TransactionOption.Transfer:

                if (!ConsoleHelper.ValidatePermission(_loggedInUser.Permissions, Permissions.Execute))
                    return;

                var transfer = new TransferScreen(_transfer);
                transfer.Run();
                break;

            case TransactionOption.ShowTransactions:

                if (!ConsoleHelper.ValidatePermission(_loggedInUser.Permissions, Permissions.Read))
                    return;

                var getAllTransactions = new GetAllTransactionsScreen(_getAllTransactions);
                getAllTransactions.Run();
                break;

            case TransactionOption.TransferLogs:

                if (!ConsoleHelper.ValidatePermission(_loggedInUser.Permissions, Permissions.Read))
                    return;

                var getAllTransfers = new GetAllTransfersScreen(_getAllTransfers);
                getAllTransfers.Run();
                break;
        }
    }
}
