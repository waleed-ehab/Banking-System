using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.ValueObjects;
using BankingSystem.Infrastructure.DataModels;
using System.Text.Json;

namespace BankingSystem.Infrastructure.Persistence;

public class JsonTransactionRepository : ITransactionRepository
{
    private readonly string _filePath;

    public JsonTransactionRepository(string filePath)
    {
        _filePath = filePath;
    }

    public bool Delete(string id)
    {
        var transactions = GetAll().ToList();

        if (transactions.RemoveAll(a => a.TransactionId == id) == 0)
        {
            return false;
        }

        WriteToFile(_filePath, transactions);

        return true;
    }

    public IEnumerable<Transaction> GetAll()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Transaction>();
        }

        var json = File.ReadAllText(_filePath);
        var dataModels = JsonSerializer.Deserialize<List<TransactionDataModel>>(json)
            ?? new List<TransactionDataModel>();

        return dataModels.Select(ToDomain);
    }

    public IEnumerable<Transaction> GetAllByClient(string clientId)
    {
        if (!File.Exists(_filePath))
        {
            return new List<Transaction>();
        }

        var json = File.ReadAllText(_filePath);
        var dataModels = JsonSerializer.Deserialize<List<TransactionDataModel>>(json)
            ?? new List<TransactionDataModel>();

        var transactionsByClient = dataModels
            .Select(ToDomain)
            .Where(t => t.ClientId == clientId);

        return transactionsByClient;
    }

    public Transaction? GetById(string id)
    {
        var transactions = GetAll().ToList();
        return transactions.FirstOrDefault(a => a.TransactionId == id);
    }

    public void Save(Transaction transaction)
    {
        var transactions = GetAll().ToList();
        var index = transactions.FindIndex(a => a.TransactionId == transaction.TransactionId);

        if (index >= 0)
        {
            transactions[index] = transaction;
        }
        else
        {
            transactions.Add(transaction);
        }

        WriteToFile(_filePath, transactions);
    }

    private static void WriteToFile(string path, IEnumerable<Transaction> transactions)
    {
        var dataModels = transactions.Select(ToDataModel);
        var json = JsonSerializer.Serialize(
            dataModels,
            new JsonSerializerOptions { WriteIndented = true }
        );

        File.WriteAllText(path, json);
    }

    private static TransactionDataModel ToDataModel(Transaction transaction)
    {
        return new TransactionDataModel
        {
            AccountId = transaction.AccountId,
            TransactionId = transaction.TransactionId,
            ClientId = transaction.ClientId,
            Amount = transaction.Amount.Amount,
            CurrencyCode = transaction.Amount.CurrencyCode,
            Date = transaction.OccurredAt,
            Type = transaction.Type.ToString()
        };
    }

    private static Transaction ToDomain(TransactionDataModel dataModel)
    {
        return Transaction.Rehydrate(
            dataModel.TransactionId,
            dataModel.AccountId,
            dataModel.ClientId,
            Balance.Create(dataModel.Amount, dataModel.CurrencyCode),
            Enum.Parse<TransactionType>(dataModel.Type),
            dataModel.Date
        );
    }

}
