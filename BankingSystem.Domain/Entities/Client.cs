using BankingSystem.Domain.Common;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.Policies;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Domain.Entities;

public class Client : User
{
    private Client(
        string id,
        string firstName,
        string lastName,
        Phone phone,
        Email email,
        Username username,
        Password password,
        UserRole role,
        Permissions permissions
    ) : base(id, firstName, lastName, phone, email, username, password, role, permissions)
    { }

    public static Client Create(
        string firstName,
        string lastName,
        Phone phone,
        Email email,
        Username username,
        Password password)
    {
        return new Client(
            EntityId.Generate(),
            firstName,
            lastName,
            phone,
            email,
            username,
            password,
            UserRole.Client,
            RolePermissions.For(UserRole.Client)
        );
    }

    public static Client Rehydrate(
        string id,
        string firstName,
        string lastName,
        Phone phone,
        Email email,
        Username username,
        Password password,
        UserRole role,
        Permissions permissions)
    {
        return new Client(id, firstName, lastName, phone, email, username, password, role, permissions);
    }

    public void Grant(Permissions permissions)
    {
        Permissions |= permissions;
    }

    public void Revoke(Permissions permissions)
    {
        Permissions &= ~permissions;
    }

    public void ResetToDefault()
    {
        Permissions = RolePermissions.For(UserRole.Client);
    }
}
