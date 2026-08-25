using BankingSystem.Application.UseCases.Clients;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.ClientManagement;

public class RegisterClientScreen
{
    private readonly RegisterClientUseCase _registerClient;

    public RegisterClientScreen(RegisterClientUseCase registerClient)
    {
        _registerClient = registerClient;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Add New Client");

        var firstName = ConsoleHelper.ReadInput("First name: ");
        var lastName = ConsoleHelper.ReadInput("Last name: ");
        var username = ConsoleHelper.ReadUsername("Username: ");
        var password = ConsoleHelper.ReadPassword("Password: ");
        var confirmPassword = ConsoleHelper.ReadPassword("Confirm password: ");
        var email = ConsoleHelper.ReadInput("Email: ");
        var phone = ConsoleHelper.ReadInput("Phone: ");

        try
        {
            if (password != confirmPassword)
            {
                Console.WriteLine("\nPasswords do not match. Please try again.");
                ConsoleHelper.WaitForUser();
                return;
            }

            var result = _registerClient.Execute(new RegisterClientRequest(firstName, lastName, username, password, email, phone));

            Console.Clear();
            ConsoleHelper.PrintClientCard(result.Id, result.FirstName, result.LastName, result.Username, result.Email, result.Phone, result.Permissions);
            Console.WriteLine("\nClient created successfully.\n");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        ConsoleHelper.WaitForUser();
    }
}
