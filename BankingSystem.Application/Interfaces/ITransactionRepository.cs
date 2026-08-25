using BankingSystem.Domain.Entities;

namespace BankingSystem.Application.Interfaces;

public interface ITransactionRepository
{
    void Save(Transaction transaction);
    bool Delete(string id);
    IEnumerable<Transaction> GetAll();
    IEnumerable<Transaction> GetAllByClient(string clientId);
    Transaction? GetById(string id);    
}
