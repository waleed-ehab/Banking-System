using BankingSystem.Application.UseCases.Authentication;
using BankingSystem.Application.UseCases.Currencies;
using BankingSystem.Domain.Enums;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.CurrencyExchange;

public class CurrencyExchangeMenuScreen
{
    public enum CurrencyExchangeOption
    {
        ListAllCurrencies = 1,
        FindCurrency = 2,
        UpdateCurrency = 3,
        CurrencyCalculator = 4,
        Exit = 0
    }

    private readonly LoginResponse _loggedInUser;
    private readonly GetAllCurrenciesUseCase _getAllCurrencies;
    private readonly GetCurrencyUseCase _getCurrency;
    private readonly UpdateCurrencyUseCase _updateCurrency;
    private readonly ConvertCurrencyUseCase _convertCurrency;

    public CurrencyExchangeMenuScreen(
        LoginResponse loggedInUser,
        GetAllCurrenciesUseCase getAllCurrencies,
        GetCurrencyUseCase getCurrency,
        UpdateCurrencyUseCase updateCurrency,
        ConvertCurrencyUseCase convertCurrency)
    {
        _loggedInUser = loggedInUser;
        _getAllCurrencies = getAllCurrencies;
        _getCurrency = getCurrency;
        _updateCurrency = updateCurrency;
        _convertCurrency = convertCurrency;
    }

    public void Run()
    {
        ConsoleHelper.RunMenu(
            PrintCurrencyExchangeMenu,
            HandleOption,
            (int)CurrencyExchangeOption.Exit,
            (int)CurrencyExchangeOption.CurrencyCalculator,
            option => (CurrencyExchangeOption)option == CurrencyExchangeOption.Exit
        );
    }

    private void HandleOption(int option)
    {
        switch ((CurrencyExchangeOption)option)
        {
            case CurrencyExchangeOption.ListAllCurrencies:
                var listAllCurrencies = new ListAllCurrenciesScreen(_getAllCurrencies);
                listAllCurrencies.Run();
                break;

            case CurrencyExchangeOption.FindCurrency:
                var findCurrency = new FindCurrencyScreen(_getCurrency);
                findCurrency.Run();
                break;

            case CurrencyExchangeOption.UpdateCurrency:

                if (!ConsoleHelper.ValidateRole(_loggedInUser.Role, UserRole.Admin))
                    return;

                var updateCurrency = new UpdateCurrencyScreen(_getCurrency, _updateCurrency);
                updateCurrency.Run();
                break;

            case CurrencyExchangeOption.CurrencyCalculator:
                var currencyCalculator = new CurrencyCalculatorScreen(_convertCurrency);
                currencyCalculator.Run();
                break;
        }
    }

    private void PrintCurrencyExchangeMenu()
    {
        var menuItems = new List<(int Number, string Text)>
        {
            (1, "List All Currencies"),
            (2, "Find Currency"),
            (3, "Update Currency"),
            (4, "Currency Calculator"),
            (0, "Exit")
        };

        ConsoleHelper.PrintMenuLayout(
            "Currency Exchange",
            () => ConsoleHelper.PrintMenu(menuItems),
            _loggedInUser.Role.ToString(),
            _loggedInUser.FullName,
            _loggedInUser.Permissions.ToString()
        );
    }
}
