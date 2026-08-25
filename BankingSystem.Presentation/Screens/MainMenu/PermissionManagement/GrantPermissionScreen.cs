using BankingSystem.Application.UseCases.PermissionManagement;
using BankingSystem.Domain.Converters;
using BankingSystem.Domain.Enums;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.PermissionManagement;

public class GrantPermissionScreen
{
    private const string Title = "Grant Permission";

    private enum GrantPermissionOption
    {
        Read = 1,
        Write = 2,
        Execute = 3,
    }

    private readonly GrantPermissionUseCase _grantPermission;
    private readonly GetClientPermissionsUseCase _getClientPermissions;

    public GrantPermissionScreen(GrantPermissionUseCase grantPermission, GetClientPermissionsUseCase getClientPermissions)
    {
        _grantPermission = grantPermission;
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

            if (!ConfirmElevation())
            {
                Console.WriteLine("\nGrant canceled. No changes were made.");
                ConsoleHelper.WaitForUser();
                return;
            }

            var permission = ToPermission((GrantPermissionOption)option);
            var result = _grantPermission.Execute(new GrantPermissionRequest(client.ClientId, permission));

            RenderClientScreen(result.ClientId, client.FullName, result.UpdatedPermissions);
            Console.WriteLine($"\n{permission} permission granted successfully.");
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
        Console.WriteLine("\nChoose a permission to grant:");
        Console.WriteLine("  1. Read");
        Console.WriteLine("  2. Write");
        Console.WriteLine("  3. Execute");
        Console.Write("  0. Cancel\n\nYour choice: ");
    }

    private bool ConfirmElevation()
    {
        var confirmation = ConsoleHelper.ReadConfirmationChar(
            $"\n{ConsoleHelper.Color("WARNING", ConsoleColorCode.Yellow)}: You are about to {ConsoleHelper.Color("elevate", ConsoleColorCode.Red)} this client's privileges." +
            $"\nAre you sure you want to proceed? {ConsoleHelper.Color("(Y/N)", ConsoleColorCode.Red)}: ");

        return confirmation is 'y' or 'Y';
    }

    private static void RenderClientScreen(string clientId, string fullName, Permissions permissions)
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader(Title);
        ConsoleHelper.PrintMiniClientCard(clientId, fullName, PermissionConverter.ToRwx(permissions));
    }

    private static Permissions ToPermission(GrantPermissionOption option) => option switch
    {
        GrantPermissionOption.Read => Permissions.Read,
        GrantPermissionOption.Write => Permissions.Write,
        GrantPermissionOption.Execute => Permissions.Execute,
        _ => Permissions.None
    };
}