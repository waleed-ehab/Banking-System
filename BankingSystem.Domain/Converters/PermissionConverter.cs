using BankingSystem.Domain.Enums;

namespace BankingSystem.Domain.Converters;

public static class PermissionConverter
{
    public static string ToRwx(Permissions permissions)
    {
        char r = permissions.HasFlag(Permissions.Read) ? 'r' : '-';
        char w = permissions.HasFlag(Permissions.Write) ? 'w' : '-';
        char x = permissions.HasFlag(Permissions.Execute) ? 'x' : '-';

        return $"{r}{w}{x}";
    }

    public static Permissions FromRwx(string rwx)
    {
        if (string.IsNullOrWhiteSpace(rwx) || rwx.Length != 3)
            throw new FormatException("Invalid rwx format.");

        Permissions permissions = Permissions.None;

        if (rwx[0] == 'r')
            permissions |= Permissions.Read;
        if (rwx[1] == 'w')
            permissions |= Permissions.Write;
        if (rwx[2] == 'x')
            permissions |= Permissions.Execute;

        return permissions;
    }
}
