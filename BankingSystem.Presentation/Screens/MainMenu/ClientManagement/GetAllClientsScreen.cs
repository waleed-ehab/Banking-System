using BankingSystem.Application.UseCases.Clients;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.ClientManagement;

public class GetAllClientsScreen
{
    private readonly GetAllClientsUseCase _getAllClients;

    public GetAllClientsScreen(GetAllClientsUseCase getAllClients)
    {
        _getAllClients = getAllClients;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader("Show Clients");

        var result = _getAllClients.Execute();

        if (!result.Clients.Any())
        {
            ConsoleHelper.PrintCenteredHeader("---- There are currently no clients in the system ----", 45, 26, false);
        }
        else
        {
            int idW = Math.Max("Id".Length, result.Clients.Max(c => c.Id.Length)) + 2;
            int fullNameW = Math.Max("Full Name".Length, result.Clients.Max(c => c.FullName.Length)) + 2;
            int emailW = Math.Max("Email".Length, result.Clients.Max(c => c.Email.Length)) + 2;
            int phoneW = Math.Max("Phone".Length, result.Clients.Max(c => c.Phone.Length)) + 2;
            int usernameW = Math.Max("Username".Length, result.Clients.Max(c => c.Username.Length)) + 2;
            int permissionsW = "Permissions".Length + 2;
            int borderW = idW + fullNameW + emailW + phoneW + usernameW + permissionsW + 7;

            Console.WriteLine($"{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
            Console.WriteLine(
                $"{ConsoleHelper.GetTabs(1)}" +
                $"|{ConsoleHelper.Color("Id".PadRight(idW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Full Name".PadRight(fullNameW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Email".PadRight(emailW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Phone".PadRight(phoneW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Username".PadRight(usernameW), ConsoleColorCode.Cyan)}" +
                $"|{ConsoleHelper.Color("Permissions".PadRight(permissionsW), ConsoleColorCode.Cyan)}|"
            );

            foreach (var client in result.Clients)
            {
                Console.WriteLine($"{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
                Console.WriteLine(
                    $"{ConsoleHelper.GetTabs(1)}" +
                    $"|{client.Id.PadRight(idW)}" +
                    $"|{client.FullName.PadRight(fullNameW)}" +
                    $"|{client.Email.PadRight(emailW)}" +
                    $"|{client.Phone.PadRight(phoneW)}" +
                    $"|{client.Username.PadRight(usernameW)}" +
                    $"|{client.Permissions.PadRight(permissionsW)}|"
                );
            }

            Console.WriteLine($"{ConsoleHelper.GetTabs(1)}{new string('-', borderW)}");
        }

        ConsoleHelper.WaitForUser();
    }
}
