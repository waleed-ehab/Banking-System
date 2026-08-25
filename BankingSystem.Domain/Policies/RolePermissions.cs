using BankingSystem.Domain.Enums;

namespace BankingSystem.Domain.Policies;

public static class RolePermissions
{
    public static Permissions For(UserRole role) => role switch
    {
        UserRole.Admin => Permissions.Read | Permissions.Write | Permissions.Execute,
        UserRole.Client => Permissions.Read | Permissions.Execute,
        _ => Permissions.None
    };

}
