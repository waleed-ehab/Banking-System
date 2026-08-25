using BankingSystem.Application.UseCases.Clients;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.ClientManagement;

public class DeleteClientScreen
{
    private readonly DeleteClientUseCase _deleteClient;
    private readonly GetClientUseCase _getClient;

    public DeleteClientScreen(DeleteClientUseCase deleteClient, GetClientUseCase getClient)
    {
        _deleteClient = deleteClient;
        _getClient = getClient;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Delete Client");

        string clientId = ConsoleHelper.ReadInput("Enter client id: ");

        try
        {
            var client = _getClient.Execute(new GetClientRequest(clientId));

            Console.Clear();
            ConsoleHelper.PrintClientCard(client.Id, client.FirstName, client.LastName, client.Username, client.Email, client.Phone, client.Permissions);

            char confirmation = ConsoleHelper.ReadConfirmationChar(
                $"\nAre you sure you want to {ConsoleHelper.Color("delete", ConsoleColorCode.Red)} this client? {ConsoleHelper.Color("(Y/N)", ConsoleColorCode.Red)}: "
            );

            if (confirmation == 'y' || confirmation == 'Y')
            {
                _deleteClient.Execute(new DeleteClientRequest(clientId));
                Console.WriteLine("\n\nClient has been deleted successfully.");
            }
            else
            {
                Console.WriteLine("\n\nDeletion canceled.");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        ConsoleHelper.WaitForUser();
    }
}
