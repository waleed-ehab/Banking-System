using BankingSystem.Application.Exceptions;
using BankingSystem.Application.UseCases.Authentication;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.Login;

public class LoginScreen
{
    private readonly LoginUseCase _login;

    public LoginScreen(LoginUseCase login)
    {
        _login = login;
    }

    public LoginResponse? Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Login");

        while (true)
        {
            var userName = ConsoleHelper.ReadUsername("Username: ");
            var password = ConsoleHelper.ReadPassword("Password: ");

            try
            {
                return _login.Execute(new LoginRequest(userName, password));
            }
            catch (UnauthorizedException ex)
            {
                Console.Clear();
                ConsoleHelper.PrintCenteredHeader("Login");
                Console.WriteLine($"\n{ex.Message}");
                Console.WriteLine("\nPress 'ESC' key to exit or any other key to try again...");

                if (Console.ReadKey(intercept: true).Key == ConsoleKey.Escape)
                    return null;

                Console.Clear();
                ConsoleHelper.PrintCenteredHeader("Login");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nUnexpected error: {ex.Message}");
            }
        }
    }
}
