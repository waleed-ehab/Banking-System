using BankingSystem.Application.UseCases.Currencies;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.CurrencyExchange;

public class ListAllCurrenciesScreen
{
    private readonly GetAllCurrenciesUseCase _getAllCurrencies;

    public ListAllCurrenciesScreen(GetAllCurrenciesUseCase getAllCurrencies)
    {
        _getAllCurrencies = getAllCurrencies;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("List All Currencies");

        var result = _getAllCurrencies.Execute();

        if (!result.Currencies.Any())
        {
            ConsoleHelper.PrintCenteredHeader("---- There are currently no currencies in the system ----", 45, 26, false);
        }
        else
        {
            int countryW = Math.Max("Country".Length, result.Currencies.Max(c => c.Country.Length)) + 2;
            int nameW = Math.Max("Name".Length, result.Currencies.Max(c => c.Name.Length)) + 2;
            int codeW = Math.Max("Code".Length, result.Currencies.Max(c => c.Code.Length)) + 2;
            int rateW = Math.Max("Rate".Length, result.Currencies.Max(c => c.Rate.ToString().Length)) + 2;
            int borderW = countryW + codeW + nameW + rateW + 5;

            Console.WriteLine($"{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
            Console.WriteLine(
                $"{ConsoleHelper.GetTabs(1)}" +
                $"|{ConsoleHelper.Color("Country".PadRight(countryW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Name".PadRight(nameW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Code".PadRight(codeW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Rate".PadRight(rateW), ConsoleColorCode.Cyan)}|"
            );

            foreach (var currency in result.Currencies)
            {
                Console.WriteLine($"{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
                Console.WriteLine(
                    $"{ConsoleHelper.GetTabs(1)}" +
                    $"|{currency.Country.PadRight(countryW)}" +
                    $"|{currency.Name.PadRight(nameW)}" +
                    $"|{currency.Code.PadRight(codeW)}" +
                    $"|{currency.Rate.ToString().PadRight(rateW)}|"
                );
            }

            Console.WriteLine($"{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
        }

        ConsoleHelper.WaitForUser();
    }
}
