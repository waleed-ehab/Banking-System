using BankingSystem.Application.UseCases.Clients;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.ClientManagement;

public class GetClientScreen
{
    private readonly GetClientUseCase _getClient;

    public GetClientScreen(GetClientUseCase getClient)
    {
        _getClient = getClient;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Get Client");

        string id = ConsoleHelper.ReadInput("Enter client id: ");

        try
        {
            var client = _getClient.Execute(new GetClientRequest(id));
            Console.Clear();
            ConsoleHelper.PrintClientCard(client.Id, client.FirstName, client.LastName, client.Username, client.Email, client.Phone, client.Permissions);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        ConsoleHelper.WaitForUser();
    }
}
