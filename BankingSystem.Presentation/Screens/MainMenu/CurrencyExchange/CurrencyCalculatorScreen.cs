using BankingSystem.Application.UseCases.Currencies;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.CurrencyExchange;

public class CurrencyCalculatorScreen
{
    private readonly ConvertCurrencyUseCase _convertCurrency;

    public CurrencyCalculatorScreen(ConvertCurrencyUseCase convertCurrency)
    {
        _convertCurrency = convertCurrency;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Currency Calculator");

        var fromCode = ConsoleHelper.ReadInput("Enter from currency: ");
        var toCode = ConsoleHelper.ReadInput("Enter to currency: ");
        var fromAmount = ConsoleHelper.ReadDecimal("Enter amount to convert: ");

        try
        {
            var result = _convertCurrency.Execute(new ConvertCurrencyRequest(fromCode, toCode, fromAmount));

            int fromAmountW = fromAmount.ToString().Length + 2;
            int fromCodeW = result.FromName.Length + 2;
            int toAmountW = result.ToAmount.ToString("F3").Length + 2;
            int toCodeW = result.ToName.Length + 2;
            int equalsW = " = ".Length;
            int borderW = fromAmountW + fromCodeW + toAmountW + toCodeW + equalsW + 3;

            Console.WriteLine($"\n{ConsoleHelper.GetTabs(2)}{new string('-', borderW)}");
            Console.WriteLine(
                $"{ConsoleHelper.GetTabs(2)}" +
                $"| {fromAmount.ToString().PadRight(fromAmountW)}" +
                $"{ConsoleHelper.Color(result.FromName.PadRight(fromCodeW), ConsoleColorCode.Cyan)}" +
                $"{"= ".PadRight(equalsW)}" +
                $"{result.ToAmount.ToString("F3").PadRight(toAmountW)}" +
                $"{ConsoleHelper.Color(result.ToName.PadRight(toCodeW), ConsoleColorCode.Cyan)}|"
            );
            Console.WriteLine($"{ConsoleHelper.GetTabs(2)}{new string('-', borderW)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        ConsoleHelper.WaitForUser();
    }
}
