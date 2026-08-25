using BankingSystem.Application.UseCases.Authentication;
using BankingSystem.Application.UseCases.PermissionManagement;
using BankingSystem.Presentation.Helpers;
using BankingSystem.Presentation.Screens.MainMenu.PermissionManagement;

namespace BankingSystem.Presentation.Screens.MainMenu;

public class PermissionManagementMenuScreen
{
    private enum PermissionManagementOption
    {
        GrantPermission = 1,
        RevokePermission = 2,
        ResetPermissionsToDefault = 3,
        Exit = 0
    }

    private readonly GrantPermissionUseCase _grantPermission;
    private readonly RevokePermissionUseCase _revokePermission;
    private readonly GetClientPermissionsUseCase _getClientPermissions;
    private readonly ResetClientPermissionsUseCase _resetClientPermissions;
    private readonly LoginResponse _loggedInUser;

    public PermissionManagementMenuScreen(GrantPermissionUseCase grantPermission, RevokePermissionUseCase revokePermission, GetClientPermissionsUseCase getClientPermissions, ResetClientPermissionsUseCase resetClientPermissions, LoginResponse loggedInUser)
    {
        _grantPermission = grantPermission;
        _revokePermission = revokePermission;
        _getClientPermissions = getClientPermissions;
        _resetClientPermissions = resetClientPermissions;
        _loggedInUser = loggedInUser;
    }

    public void Run()
    {
        ConsoleHelper.RunMenu(
            PrintClientManagementMenu,
            HandleOption,
            (int)PermissionManagementOption.Exit,
            (int)PermissionManagementOption.ResetPermissionsToDefault,
            option => (PermissionManagementOption)option == PermissionManagementOption.Exit
        );
    }

    private void PrintClientManagementMenu()
    {
        var menuItems = new List<(int Number, string Text)>
        {
            (1, "Grant Permission"),
            (2, "Revoke Permission"),
            (3, "Reset Permissions To Default"),
            (0, "Exit")
        };

        ConsoleHelper.PrintMenuLayout(
            "Permission Management",
            () => ConsoleHelper.PrintMenu(menuItems),
            _loggedInUser.Role.ToString(),
            _loggedInUser.FullName,
            _loggedInUser.Permissions.ToString()
        );
    }

    private void HandleOption(int option)
    {
        switch ((PermissionManagementOption)option)
        {
            case PermissionManagementOption.GrantPermission:
                var grantPermission = new GrantPermissionScreen(_grantPermission, _getClientPermissions);
                grantPermission.Run();
                break;

            case PermissionManagementOption.RevokePermission:
                var revokePermission = new RevokePermissionScreen(_revokePermission, _getClientPermissions);
                revokePermission.Run();
                break;

            case PermissionManagementOption.ResetPermissionsToDefault:
                var resetToDefault = new ResetToDefaultScreen(_resetClientPermissions, _getClientPermissions);
                resetToDefault.Run();
                break;
        }
    }
}
