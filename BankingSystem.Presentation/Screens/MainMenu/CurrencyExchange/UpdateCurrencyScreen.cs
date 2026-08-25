using BankingSystem.Application.UseCases.Currencies;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.CurrencyExchange;

public class UpdateCurrencyScreen
{
    private readonly GetCurrencyUseCase _getCurrency;
    private readonly UpdateCurrencyUseCase _updateCurrency;

    public UpdateCurrencyScreen(GetCurrencyUseCase getCurrency, UpdateCurrencyUseCase updateCurrency)
    {
        _getCurrency = getCurrency;
        _updateCurrency = updateCurrency;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Update Currency");

        string code = ConsoleHelper.ReadInput("Enter currency code (e.g., USD, EUR, EGP): ");

        try
        {
            var currency = _getCurrency.Execute(new GetCurrencyRequest(code));

            ConsoleHelper.PrintCurrencyCard(currency.Country, currency.Name, currency.Code, currency.Rate);

            var rate = ConsoleHelper.ReadDecimal("Enter currency rate: ");
            var result = _updateCurrency.Execute(new UpdateCurrencyRequest(currency.Code, rate));

            Console.Clear();
            Console.WriteLine("\nCurrency has been updated successfully.");

            ConsoleHelper.PrintCurrencyCard(result.Country, result.Name, result.Code, result.Rate);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        ConsoleHelper.WaitForUser();
    }
}
