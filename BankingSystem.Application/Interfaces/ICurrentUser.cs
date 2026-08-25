using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.Interfaces;

public interface ICurrentUser
{
    string Id { get; }
    UserRole Role { get; }
    Permissions Permissions { get; }
}
