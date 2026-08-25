using BankingSystem.Application.UseCases.Clients;
using BankingSystem.Presentation.Helpers;

namespace BankingSystem.Presentation.Screens.MainMenu.ClientManagement;

public class UpdateClientScreen
{
    private const string Title = "Update Client";

    private enum UpdateOptions
    {
        FirstName = 0,
        LastName = 1,
        Email = 2,
        Phone = 3,
        Save = 4
    }

    private readonly UpdateClientUseCase _updateClient;
    private readonly GetClientUseCase _getClient;

    public UpdateClientScreen(UpdateClientUseCase updateClient, GetClientUseCase getClient)
    {
        _updateClient = updateClient;
        _getClient = getClient;
    }

    public void Run()
    {
        Console.Clear();
        ConsoleHelper.PrintCenteredHeader(Title);
        string id = ConsoleHelper.ReadInput("Enter client id: ");

        try
        {
            var client = _getClient.Execute(new GetClientRequest(id));

            string newFirstName = client.FirstName;
            string newLastName = client.LastName;
            string newEmail = client.Email;
            string newPhone = client.Phone;

            UpdateOptions option;
            do
            {
                option = ReadUpdateOption(client, newFirstName, newLastName, newEmail, newPhone);
                RenderClientCard(client, newFirstName, newLastName, newEmail, newPhone);

                switch (option)
                {
                    case UpdateOptions.FirstName:
                        newFirstName = ConsoleHelper.ReadInput("Enter first name: ");
                        break;
                    case UpdateOptions.LastName:
                        newLastName = ConsoleHelper.ReadInput("Enter last name: ");
                        break;
                    case UpdateOptions.Email:
                        newEmail = ConsoleHelper.ReadInput("Enter email: ");
                        break;
                    case UpdateOptions.Phone:
                        newPhone = ConsoleHelper.ReadInput("Phone: ");
                        break;
                }
            } while (option != UpdateOptions.Save);

            RenderClientCard(client, newFirstName, newLastName, newEmail, newPhone);

            if (ConfirmUpdate())
            {
                _updateClient.Execute(new UpdateClientRequest(client.Id, newFirstName, newLastName, newPhone, newEmail));
                Console.WriteLine("\n\nClient has been updated successfully.");
            }
            else
            {
                Console.WriteLine("\n\nUpdate canceled.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n\nError: {ex.Message}");
        }

        ConsoleHelper.WaitForUser();
    }

    private UpdateOptions ReadUpdateOption(GetClientResponse client, string firstName, string lastName, string email, string phone)
    {
        RenderClientCard(client, firstName, lastName, email, phone);
        PrintUpdateOptions();
        Console.Write("\nEnter an update option: ");

        int option;
        while (!int.TryParse(Console.ReadLine(), out option) || option < 0 || option > 4)
        {
            RenderClientCard(client, firstName, lastName, email, phone);
            PrintUpdateOptions();
            Console.Write("\nInvalid option, please enter an option between 0 and 4: ");
        }

        return (UpdateOptions)option;
    }

    private bool ConfirmUpdate()
    {
        var confirmation = ConsoleHelper.ReadConfirmationChar(
            $"\n{ConsoleHelper.Color("WARNING", ConsoleColorCode.Yellow)}: You are about to {ConsoleHelper.Color("update", ConsoleColorCode.Red)} this client's data." +
            $"\nAre you sure you want to proceed? {ConsoleHelper.Color("(Y/N)", ConsoleColorCode.Red)}: ");

        return confirmation is 'y' or 'Y';
    }

    private static void RenderClientCard(GetClientResponse client, string firstName, string lastName, string email, string phone)
    {
        Console.Clear();
        ConsoleHelper.PrintClientCard(client.Id, firstName, lastName, client.Username, email, phone, client.Permissions);
    }

    private void PrintUpdateOptions()
    {
        Console.WriteLine("\nUpdate Options:\n");
        Console.WriteLine("0. First Name");
        Console.WriteLine("1. Last Name");
        Console.WriteLine("2. Email");
        Console.WriteLine("3. Phone");
        Console.WriteLine("4. Save");
    }
}