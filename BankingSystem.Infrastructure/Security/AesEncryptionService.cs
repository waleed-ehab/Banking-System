using BankingSystem.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

public class AesEncryptionService : IEncryptionService
{
    private readonly byte[] _key;

    public AesEncryptionService(string key)
    {
        _key = Convert.FromBase64String(key);   

        if (_key.Length != 32)
            throw new ArgumentException("Key must be 32 bytes.");
    }

    public string Encrypt(string plainText)
    {
        byte[] nonce = RandomNumberGenerator.GetBytes(12);
        byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);

        byte[] ciphertext = new byte[plaintextBytes.Length];
        byte[] tag = new byte[16];

        using var aes = new AesGcm(_key, 16);

        aes.Encrypt(
            nonce,
            plaintextBytes,
            ciphertext,
            tag
        );

        byte[] result = new byte[nonce.Length + tag.Length + ciphertext.Length];

        Buffer.BlockCopy(nonce, 0, result, 0, nonce.Length);
        Buffer.BlockCopy(tag, 0, result, nonce.Length, tag.Length);
        Buffer.BlockCopy(ciphertext, 0, result, nonce.Length + tag.Length, ciphertext.Length);

        return Convert.ToBase64String(result);
    }

    public string Decrypt(string cipherText)
    {
        byte[] data = Convert.FromBase64String(cipherText);

        byte[] nonce = data[..12];
        byte[] tag = data[12..28];
        byte[] ciphertext = data[28..];

        byte[] plaintext = new byte[ciphertext.Length];

        using var aes = new AesGcm(_key, 16);

        aes.Decrypt(
            nonce,
            ciphertext,
            tag,
            plaintext
        );

        return Encoding.UTF8.GetString(plaintext);
    }
}