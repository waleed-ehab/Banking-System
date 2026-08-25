using BankingSystem.Application.UseCases.PermissionManagement;
using BankingSystem.Domain.Converters;
using BankingSystem.Domain.Enums;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.PermissionManagement;

public class ResetToDefaultScreen
{
    private const string Title = "Reset Permissions to Default";

    private readonly ResetClientPermissionsUseCase _resetClientPermissions;
    private readonly GetClientPermissionsUseCase _getClientPermissions;

    public ResetToDefaultScreen(ResetClientPermissionsUseCase resetClientPermissions, GetClientPermissionsUseCase getClientPermissions)
    {
        _resetClientPermissions = resetClientPermissions;
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

            RenderClientScreen(client.ClientId, client.FullName, client.Permissions);

            if (!ConfirmReset())
            {
                Console.WriteLine("\nReset canceled. No changes were made.");
                ConsoleHelper.WaitForUser();
                return;
            }

            var result = _resetClientPermissions.Execute(new ResetClientPermissionsRequest(client.ClientId));

            RenderClientScreen(result.ClientId, client.FullName, result.UpdatedPermissions);
            Console.WriteLine($"\nPermissions reset to default ({PermissionConverter.ToRwx(result.UpdatedPermissions)}).");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        ConsoleHelper.WaitForUser();
    }

    private bool ConfirmReset()
    {
        var confirmation = ConsoleHelper.ReadConfirmationChar(
            $"\n{ConsoleHelper.Color("WARNING", ConsoleColorCode.Yellow)}: You are about to {ConsoleHelper.Color("reset", ConsoleColorCode.Red)} this client's permissions to the role default." +
            $"\nAre you sure you want to proceed? {ConsoleHelper.Color("(Y/N)", ConsoleColorCode.Red)}: ");

        return confirmation is 'y' or 'Y';
    }

    private static void RenderClientScreen(string clientId, string fullName, Permissions permissions)
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader(Title);
        ConsoleHelper.PrintMiniClientCard(clientId, fullName, PermissionConverter.ToRwx(permissions));
    }
}