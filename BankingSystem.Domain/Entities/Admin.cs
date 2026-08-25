using BankingSystem.Domain.Common;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.Policies;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Domain.Entities;

public class Admin : User
{
    private Admin(
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

    public static Admin Create(
        string firstName,
        string lastName,
        Phone phone,
        Email email,
        Username username,
        Password password)
    {
        return new Admin(
            EntityId.Generate(),
            firstName,
            lastName,
            phone,
            email,
            username,
            password,
            UserRole.Admin,
            RolePermissions.For(UserRole.Admin)
        );
    }

    public static Admin Rehydrate(
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
        return new Admin(id, firstName, lastName, phone, email, username, password, role, permissions);
    }
}
