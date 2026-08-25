namespace BankingSystem.Domain.ValueObjects;

public class Pin
{
    public string EncryptedPin { get; }

    private Pin(string encryptedPin)
    {
        EncryptedPin = encryptedPin;
    }

    public static Pin FromEncryptedText(string encryptedPin)
    {
        return new Pin(encryptedPin);
    }
}
