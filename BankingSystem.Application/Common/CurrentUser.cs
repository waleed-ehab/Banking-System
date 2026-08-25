using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.Common;

public record CurrentUser : ICurrentUser
{
    public string Id { get; init; }

    public UserRole Role { get; init; }

    public Permissions Permissions { get; init; }

    public CurrentUser(string id, UserRole role, Permissions permissions)
    {
        Id = id;
        Role = role;
        Permissions = permissions;
    }
}