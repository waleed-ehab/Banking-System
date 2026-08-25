using BankingSystem.Application.UseCases.Authentication;
using BankingSystem.Application.UseCases.Clients;
using BankingSystem.Domain.Enums;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.ClientManagement;

public class ClientManagmentMenuScreen
{
    private enum ClientManagementOption
    {
        ShowClients = 1,
        AddNewClient = 2,
        DeleteClient = 3,
        UpdateClient = 4,
        FindClient = 5,
        Exit = 0
    }

    private readonly LoginResponse _loggedInUser;
    private readonly DeleteClientUseCase _deleteClient;
    private readonly GetAllClientsUseCase _getAllClients;
    private readonly GetClientUseCase _getClient;
    private readonly RegisterClientUseCase _registerClient;
    private readonly UpdateClientUseCase _updateClient;

    public ClientManagmentMenuScreen(
        LoginResponse loggedInUser,
        DeleteClientUseCase deleteClient,
        GetAllClientsUseCase getAllClients,
        GetClientUseCase getClient,
        RegisterClientUseCase registerClient,
        UpdateClientUseCase updateClient)
    {
        _loggedInUser = loggedInUser;
        _deleteClient = deleteClient;
        _getAllClients = getAllClients;
        _getClient = getClient;
        _registerClient = registerClient;
        _updateClient = updateClient;
    }

    public void Run()
    {
        ConsoleHelper.RunMenu(
            PrintClientManagementMenu,
            HandleOption,
            (int)ClientManagementOption.Exit,
            (int)ClientManagementOption.FindClient,
            option => (ClientManagementOption)option == ClientManagementOption.Exit
        );
    }

    private void PrintClientManagementMenu()
    {
        var menuItems = new List<(int Number, string Text)>
        {
            (1, "Show Clients"),
            (2, "Add New Client"),
            (3, "Delete Client"),
            (4, "Update Client"),
            (5, "Find Client"),
            (0,  "Exit")
        };

        ConsoleHelper.PrintMenuLayout(
            "Client Management", 
            () => ConsoleHelper.PrintMenu(menuItems),
            _loggedInUser.Role.ToString(), 
            _loggedInUser.FullName, 
            _loggedInUser.Permissions.ToString()
        );
    }

    private void HandleOption(int option)
    {
        switch ((ClientManagementOption)option)
        {
            case ClientManagementOption.ShowClients:
                var getAllClients = new GetAllClientsScreen(_getAllClients);
                getAllClients.Run();
                break;

            case ClientManagementOption.AddNewClient:
                var registerClient = new RegisterClientScreen(_registerClient);
                registerClient.Run();
                break;

            case ClientManagementOption.DeleteClient:
                var deleteClient = new DeleteClientScreen(_deleteClient, _getClient);
                deleteClient.Run();
                break;

            case ClientManagementOption.UpdateClient:
                var updateClient = new UpdateClientScreen(_updateClient, _getClient);
                updateClient.Run();
                break;

            case ClientManagementOption.FindClient:
                var findClient = new GetClientScreen(_getClient);
                findClient.Run();
                break;
        }
    }
}

