namespace BankingSystem.Domain.Common;

public static class EntityId
{
    public static string Generate() => Guid.NewGuid().ToString()[..8];
}
