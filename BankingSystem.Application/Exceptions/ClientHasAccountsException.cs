namespace BankingSystem.Application.Exceptions;

public class ClientHasAccountsException : Exception
{
    public ClientHasAccountsException(string message)
        : base(message) { }
}
