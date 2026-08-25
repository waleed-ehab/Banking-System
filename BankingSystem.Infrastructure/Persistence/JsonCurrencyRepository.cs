using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.ValueObjects;
using BankingSystem.Infrastructure.DataModels;
using System.Text.Json;

namespace BankingSystem.Infrastructure.Persistence;

public class JsonCurrencyRepository : ICurrencyRepository
{
    private readonly string _filepath;

    public JsonCurrencyRepository(string filePath)
    {
        _filepath = filePath;
    }

    public IEnumerable<Currency> GetAll()
    {
        if (!File.Exists(_filepath))
        {
            return new List<Currency>();
        }

        var json = File.ReadAllText(_filepath);
        var currencies = JsonSerializer.Deserialize<IEnumerable<CurrencyDataModel>>(json)
            ?? new List<CurrencyDataModel>();

        return currencies.Select(ToDomain);
    }

    public Currency? GetByCode(string code)
    {
        code = code.ToUpper();
        var currencies = GetAll();
        return currencies.FirstOrDefault(c => c.Code == code);
    }

    public void Save(Currency currency)
    {
        var currencies = GetAll().ToList();
        int index = currencies.FindIndex(c => c.Code == currency.Code);

        if (index >= 0)
        {
            currencies[index] = currency;
        }
        else
        {
            currencies.Add(currency);
        }

        WriteToFile(_filepath, currencies);
    }

    public void InsertRange(IEnumerable<Currency> currencies)
    {
        var existing = GetAll().ToList();

        var newItems = currencies
            .Where(c => !existing.Any(e => e.Code == c.Code))
            .ToList();

        if (!newItems.Any())
        {
            return;
        }

        existing.AddRange(newItems);

        WriteToFile(_filepath, existing);
    }

    private static void WriteToFile(string path, IEnumerable<Currency> currencies)
    {
        var dataModels = currencies.Select(ToDataModel);
        var json = JsonSerializer.Serialize(
            dataModels,
            new JsonSerializerOptions { WriteIndented = true }
        );

        File.WriteAllText(path, json);
    }

    private static CurrencyDataModel ToDataModel(Currency currency)
    {
        return new CurrencyDataModel
        {
            Country = currency.Country,
            Code = currency.Code,
            Name = currency.Name,
            Rate = currency.Rate
        };
    }

    private static Currency ToDomain(CurrencyDataModel dataModel)
    {
        return Currency.Rehydrate(
            dataModel.Country,
            dataModel.Code,
            dataModel.Name,
            dataModel.Rate
        );
    }
}
