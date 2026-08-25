using BankingSystem.Application.UseCases.PermissionManagement;
using BankingSystem.Domain.Converters;
using BankingSystem.Domain.Enums;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.PermissionManagement;

public class RevokePermissionScreen
{
    private const string Title = "Revoke Permission";

    private enum RevokePermissionOption
    {
        Read = 1,
        Write = 2,
        Execute = 3,
    }

    private readonly RevokePermissionUseCase _revokePermission;
    private readonly GetClientPermissionsUseCase _getClientPermissions;

    public RevokePermissionScreen(RevokePermissionUseCase revokePermission, GetClientPermissionsUseCase getClientPermissions)
    {
        _revokePermission = revokePermission;
        _getClientPermissions = getClientPermissions;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader(Title);
        string id = ConsoleHelper.ReadInput("Enter client id: ");

        try
        {
            var client = _getClientPermissions.Execute(new GetClientPermissionsRequest(id));

            int option = ReadPermissionOption(client.ClientId, client.FullName, client.Permissions);
            if (option == 0)
                return;

            RenderClientScreen(client.ClientId, client.FullName, client.Permissions);

            if (!ConfirmRevocation())
            {
                Console.WriteLine("\nRevoke canceled. No changes were made.");
                ConsoleHelper.WaitForUser();
                return;
            }

            var permission = ToPermission((RevokePermissionOption)option);
            var result = _revokePermission.Execute(new RevokePermissionRequest(client.ClientId, permission));

            RenderClientScreen(result.ClientId, client.FullName, result.UpdatedPermissions);
            Console.WriteLine($"\n{permission} permission revoked successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        ConsoleHelper.WaitForUser();
    }

    private int ReadPermissionOption(string clientId, string fullName, Permissions permissions)
    {
        RenderClientScreen(clientId, fullName, permissions);
        PrintOptionPrompt();

        int option;
        while (!int.TryParse(Console.ReadLine(), out option) || option < 0 || option > 3)
        {
            RenderClientScreen(clientId, fullName, permissions);
            Console.WriteLine($"\n{ConsoleHelper.Color("Invalid option.", ConsoleColorCode.Red)} Please enter a number between 0 and 3.\n");
            PrintOptionPrompt();
        }

        return option;
    }

    private static void PrintOptionPrompt()
    {
        Console.WriteLine("\nChoose a permission to revoke:");
        Console.WriteLine("  1. Read");
        Console.WriteLine("  2. Write");
        Console.WriteLine("  3. Execute");
        Console.Write("  0. Cancel\n\nYour choice: ");
    }

    private bool ConfirmRevocation()
    {
        var confirmation = ConsoleHelper.ReadConfirmationChar(
            $"\n{ConsoleHelper.Color("WARNING", ConsoleColorCode.Yellow)}: You are about to {ConsoleHelper.Color("restrict", ConsoleColorCode.Red)} this client's privileges." +
            $"\nAre you sure you want to proceed? {ConsoleHelper.Color("(Y/N)", ConsoleColorCode.Red)}: ");

        return confirmation is 'y' or 'Y';
    }

    private static void RenderClientScreen(string clientId, string fullName, Permissions permissions)
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader(Title);
        ConsoleHelper.PrintMiniClientCard(clientId, fullName, PermissionConverter.ToRwx(permissions));
    }

    private static Permissions ToPermission(RevokePermissionOption option) => option switch
    {
        RevokePermissionOption.Read => Permissions.Read,
        RevokePermissionOption.Write => Permissions.Write,
        RevokePermissionOption.Execute => Permissions.Execute,
        _ => Permissions.None
    };
}