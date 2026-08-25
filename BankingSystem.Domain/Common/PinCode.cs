namespace BankingSystem.Domain.Common;

public class PinCode
{
    private const int Length = 4;

    private static readonly Random random = new();

    public static string Generate()
    {
        var pin = random.Next(0, 10000).ToString($"D{Length}");
        return pin;
    }
}
