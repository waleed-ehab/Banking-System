using BankingSystem.Application.UseCases.Currencies;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.CurrencyExchange;

public class FindCurrencyScreen
{
    private readonly GetCurrencyUseCase _getCurrency;

    public FindCurrencyScreen(GetCurrencyUseCase getCurrency)
    {
        _getCurrency = getCurrency;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Find Currency");

        string code = ConsoleHelper.ReadInput("Enter currency code (e.g., USD, EUR, EGP): ");

        try
        {
            var result = _getCurrency.Execute(new GetCurrencyRequest(code));
            ConsoleHelper.PrintCurrencyCard(result.Country, result.Name, result.Code, result.Rate);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        ConsoleHelper.WaitForUser();
    }

}
