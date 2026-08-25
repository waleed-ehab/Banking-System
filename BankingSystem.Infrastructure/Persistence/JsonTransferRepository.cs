using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.ValueObjects;
using BankingSystem.Infrastructure.DataModels;
using System.Text.Json;

namespace BankingSystem.Infrastructure.Persistence;

public class JsonTransferRepository : ITransferRepository
{
    private readonly string _filePath;

    public JsonTransferRepository(string filePath)
    {
        _filePath = filePath;
    }

    public IEnumerable<Transfer> GetAll()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Transfer>();
        }

        var json = File.ReadAllText(_filePath);
        var dataModels = JsonSerializer.Deserialize<List<TransferDataModel>>(json)
            ?? new List<TransferDataModel>();

        return dataModels.Select(ToDomain);
    }

    public IEnumerable<Transfer> GetByAccountIds(IEnumerable<string> accountIds)
    {
        if (!File.Exists(_filePath))
        {
            return new List<Transfer>();
        }

        var json = File.ReadAllText(_filePath);
        var dataModels = JsonSerializer.Deserialize<List<TransferDataModel>>(json)
            ?? new List<TransferDataModel>();

        var transfersBySourceAccountIds = dataModels
            .Select(ToDomain)
            .Where(t => 
                accountIds.Contains(t.SourceAccountId) ||
                accountIds.Contains(t.DestinationAccountId)
        );

        return transfersBySourceAccountIds;
    }

    public void Save(Transfer transfer)
    {
        var transfers = GetAll().ToList();

        transfers.Add(transfer);

        WriteToFile(_filePath, transfers);
    }

    private static void WriteToFile(string path, IEnumerable<Transfer> transfers)
    {
        var dataModels = transfers.Select(ToDataModel);
        var json = JsonSerializer.Serialize(
            dataModels,
            new JsonSerializerOptions { WriteIndented = true }
        );

        File.WriteAllText(path, json);
    }

    private static TransferDataModel ToDataModel(Transfer transfer)
    {
        return new TransferDataModel
        {
            SourceAccountId = transfer.SourceAccountId,
            DestinationAccountId = transfer.DestinationAccountId,
            Amount = transfer.Amount.Amount,
            CurrencyCode = transfer.Amount.CurrencyCode,
            Date = transfer.OccurredAt
        };
    }

    private static Transfer ToDomain(TransferDataModel dataModel)
    {
        return Transfer.Rehydrate(
            dataModel.SourceAccountId,
            dataModel.DestinationAccountId,
            Balance.Create(dataModel.Amount, dataModel.CurrencyCode),
            dataModel.Date
        );
    }

}
