using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.ValueObjects;
using BankingSystem.Domain.DataModels;
using System.Text.Json;

namespace BankingSystem.Domain.Persistence;

public class JsonAccountRepository : IAccountRepository
{
    private readonly string _filePath;

    public JsonAccountRepository(string filePath)
    {
        _filePath = filePath;
    }

    public bool Delete(string id)
    {
        var accounts = GetAll().ToList();
        var accountToDelete = accounts.FirstOrDefault(a => a.AccountId == id);

        if (accountToDelete is null)
        {
            return false;
        }

        accountToDelete.Delete();

        WriteToFile(_filePath, accounts);

        return true;
    }

    public IEnumerable<Account> GetAll()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Account>();
        }

        var json = File.ReadAllText(_filePath);
        var dataModels = JsonSerializer.Deserialize<List<AccountDataModel>>(json)
            ?? new List<AccountDataModel>();

        return dataModels.Select(ToDomain);
    }

    public IEnumerable<Account> GetAllByClient(string userId)
    {
        var accountsByClient = GetAll()
            .Where(a => a.UserId == userId);

        return accountsByClient;
    }

    public Account? GetById(string id)
    {
        var accounts = GetAll().ToList();
        return accounts.FirstOrDefault(a => a.AccountId == id);
    }

    public void Save(Account account)
    {
        var accounts = GetAll().ToList();
        var index = accounts.FindIndex(a => a.AccountId == account.AccountId);

        if (index >= 0)
        {
            accounts[index] = account;
        }
        else
        {
            accounts.Add(account);
        }

        WriteToFile(_filePath, accounts);
    }

    private static void WriteToFile(string path, IEnumerable<Account> accounts)
    {
        var dataModels = accounts.Select(ToDataModel);
        var json = JsonSerializer.Serialize(
            dataModels,
            new JsonSerializerOptions { WriteIndented = true }
        );

        File.WriteAllText(path, json);
    }

    private static AccountDataModel ToDataModel(Account account)
    {
        return new AccountDataModel
        {
            AccountId = account.AccountId,
            CurrencyCode = account.Balance.CurrencyCode,
            DeletedAt = account.DeletedAt,
            IsDeleted = account.IsDeleted,
            Balance = account.Balance.Amount,
            PinHash = account.Pin.EncryptedPin,
            IsLocked = account.IsLocked,
            FailedAttempts = account.FailedPinAttempts,
            LockedUntil = account.LockedUntil,
            UserId = account.UserId
        };
    }

    private static Account ToDomain(AccountDataModel dataModel)
    {
        return Account.Rehydrate(
            dataModel.AccountId,
            Pin.FromEncryptedText(dataModel.PinHash),
            dataModel.UserId,
            Balance.Create(dataModel.Balance, dataModel.CurrencyCode),
            dataModel.IsLocked,
            dataModel.FailedAttempts,
            dataModel.LockedUntil,
            dataModel.IsDeleted,
            dataModel.DeletedAt
        );
    }
}
