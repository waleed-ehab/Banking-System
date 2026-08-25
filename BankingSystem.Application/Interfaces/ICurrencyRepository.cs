using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Application.Interfaces;

public interface ICurrencyRepository
{
    void Save(Currency currency);
    void InsertRange(IEnumerable<Currency> currencies);
    Currency? GetByCode(string code);
    IEnumerable<Currency> GetAll();
}
